using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using TemplateWebAPI.Infrastructure.Repositories.Contracts;

namespace TemplateWebAPI.Infrastructure.Repositories
{
    /// <summary>
    ///     Esta classe é responsável pelas comunicação/transações com o banco de 
    ///     dados Microsoft SQL Server (2014 ou superior).
    /// </summary>
    public sealed class SqlServerRepository : ISqlServerRepository
    {
        #region SqlServerRepository - Fields

        /// <summary>
        ///     Campo que armazena uma flag que informa se o método Dispose() já foi executado.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        ///     Campo que armazena uma flag que informa se o método Dispose() já foi executado.
        /// </summary>
        private bool isTransaction = false;

        /// <summary>
        ///     Campo que armazena objeto do tipo <see cref="IDataReader"/> que 
        ///     realiza a leitura e retorno de registros na base de dados.
        /// </summary>
        private IDataReader dataReader = null;

        /// <summary>
        ///     Campo que armazena objeto do tipo <see cref="IDbCommand"/> que 
        ///     realiza a execução de comandos SQL na base de dados.
        /// </summary>
        private IDbCommand dbCommand = null;

        /// <summary>
        ///     Campo que armazena objeto do tipo <see cref="IDbConnection"/> que é 
        ///     responsável por estabelecer uma conexão com uma base de dados.
        /// </summary>
        private IDbConnection dbConnection = null;

        /// <summary>
        ///     Campo que armazena objeto do tipo <see cref="IDbTransaction"/> que 
        ///     é responsável por estabelecer transações com uma base de dados.
        /// </summary>
        private IDbTransaction dbTransaction = null;

        #endregion

        #region SqlServerRepository - Properties

        /// <summary>
        /// Propriedade que recebe e/ou retorna o <see cref="IDbCommand"/> que comanda as operações no banco de dados.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public IDbCommand Command
        {
            get
            {
                return dbCommand;
            }
        }

        #endregion

        #region SqlServerRepository - Manutenção da classe

        /// <summary>
        ///     Inicializador da classe. Deve se passar a string de conexão como parâmetro obrigatório.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        public SqlServerRepository(string connectionString)
        {
            dbConnection = null;
            dbCommand = dbConnection.CreateCommand();
        }

        /// <summary>
        ///     Inicializador da classe. Deve se passar a string de conexão como parâmetro obrigatório.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public SqlServerRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
            dbCommand = dbConnection.CreateCommand();
        }

        /// <summary>
        ///     Destrutor da classe.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~SqlServerRepository()
        {
            Dispose(disposed);
        }

        /// <summary>
        ///     Este método libera os recursos pendentes dentro da classe.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposed);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Este método libera os recursos pendentes dentro da classe. Somente é usado com processos internos da classe.
        /// </summary>
        /// <param name="isDisposed">
        ///     Usado para indicar se o método dispose ja foi executado internamente.
        /// </param>
        private void Dispose(bool isDisposed)
        {
            if (!isDisposed)
            {
                if (dataReader != null)
                    if (!dataReader.IsClosed)
                        dataReader.Close();

                if (dbTransaction != null)
                    dbTransaction.Dispose();

                if (dbCommand != null)
                    dbCommand.Dispose();

                if (dbConnection != null)
                {
                    if (dbConnection.State != ConnectionState.Closed)
                        dbConnection.Close();

                    dbConnection.Dispose();
                }

                dbConnection = null;
                dbTransaction = null;
                dbCommand = null;
                dataReader = null;

                disposed = true;
            }
        }

        #endregion

        #region SqlServerRepository - Métodos

        /// <summary>
        /// Este método adiciona parâmetro para a coleção de parâmetros do campo <see cref="dbCommand"/> da classe.
        /// </summary>
        public void AddParameter(string parameterName, object parameterValue, ParameterDirection parameterDirection, SqlDbType sqlDbType)
        {
            SqlParameter sqlParameter = new SqlParameter(parameterName, sqlDbType);
            sqlParameter.Direction = parameterDirection;
            sqlParameter.Value = (parameterValue == null) ? DBNull.Value : parameterValue;

            dbCommand.Parameters.Add(sqlParameter);
        }

        /// <summary>
        ///     teste
        /// </summary>
        /// <returns>
        ///     Retorna um objeto do tipo <see cref="DbResult{T}"/>.
        /// </returns>
        public DbResult<bool> BeginTransaction(IsolationLevel isolationLevel)
        {
            try
            {
                if (dbConnection.State != ConnectionState.Closed)
                    return DbResultFactory.CreateDbErrorResult<bool>(new Exception("Já existe uma conexão aberta."));

                dbConnection.Open();

                dbTransaction = dbConnection.BeginTransaction(isolationLevel);

                dbCommand.Transaction = dbTransaction;

                isTransaction = true;
                
                return DbResultFactory.CreateDbResult(true);
            }
            catch (Exception exception)
            {
                try
                {
                    if (dbTransaction != null)
                        dbTransaction.Rollback();

                    if (dbConnection != null)
                    {
                        if (dbConnection.State != ConnectionState.Closed)
                            dbConnection.Close();
                    }
                }
                catch (Exception rollbackException)
                {
                    return DbResultFactory.CreateDbErrorResult<bool>(new ApplicationException(string.Format("Error at [{0} - [{1}] : {2}",
                        MethodBase.GetCurrentMethod().Name,
                        rollbackException.GetType(),
                        rollbackException.Message)));
                }

                return DbResultFactory.CreateDbErrorResult<bool>(exception);
            }
        }

        /// <summary>
        /// Este método encerra uma transação na base de dados.
        /// </summary>
        /// <param name="success">Indica se deverá ser executado o ROLLBACK ou COMMIT na transação.</param>
        /// <returns>
        ///     Retorna um objeto do tipo <see cref="DbResult{T}"/>.
        /// </returns>
        public DbResult<bool> EndTransaction(bool success)
        {
            try
            {
                if (dataReader != null)
                    if (!dataReader.IsClosed)
                        dataReader.Close();

                if (dbTransaction != null)
                {
                    if (success)
                        dbTransaction.Commit();
                    else
                        dbTransaction.Rollback();
                }

                return DbResultFactory.CreateDbResult(true);
            }
            catch (Exception exception)
            {
                try
                {
                    if (dbTransaction != null)
                        dbTransaction.Rollback();
                }
                catch (Exception rollbackException)
                {
                    return DbResultFactory.CreateDbErrorResult<bool>(new ApplicationException(string.Format("Error at [{0} - [{1}] : {2}",
                        MethodBase.GetCurrentMethod().Name,
                        rollbackException.GetType(),
                        rollbackException.Message)));
                }

                return DbResultFactory.CreateDbErrorResult<bool>(exception);
            }
            finally
            {
                isTransaction = false;

                if (dbConnection != null)
                    if (dbConnection.State != ConnectionState.Closed)
                        dbConnection.Close();
            }
        }

        /// <summary>
        /// Este método executa instruções SQL na base de dados e não retorna registros.
        /// </summary>
        /// <remarks>Este método é recomendado para e execução de comandos SQL com INSERT, UPDATE E DELETE.</remarks>
        public DbResult<int> ExecuteNonQuery(string commandText)
        {
            try
            {
                if (!isTransaction)
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                dbCommand.CommandText = commandText;
                dbCommand.CommandType = CommandType.StoredProcedure;

                return DbResultFactory.CreateDbResult(dbCommand.ExecuteNonQuery());
            }
            catch (Exception exception)
            {
                return DbResultFactory.CreateDbErrorResult<int>(exception);
            }
            finally
            {
                if (!isTransaction)
                {
                    if (dbConnection.State != ConnectionState.Closed)
                        dbConnection.Close();
                }

                if (dbCommand != null)
                    dbCommand.Parameters.Clear();
            }
        }

        /// <summary>
        /// Este método executa instruções SQL na base de dados e retorna um registro simples.
        /// </summary>
        /// <returns>Retorna um objeto do tipo <see cref="Object"/>.</returns>
        public DbResult<object> ExecuteScalar(string commandText)
        {
            try
            {
                if (!isTransaction)
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                dbCommand.CommandText = commandText;
                dbCommand.CommandType = CommandType.StoredProcedure;

                return DbResultFactory.CreateDbResult(dbCommand.ExecuteScalar());
            }
            catch (Exception exception)
            {
                return DbResultFactory.CreateDbErrorResult<object>(exception);
            }
            finally
            {
                if (!isTransaction)
                {
                    if (dbConnection.State != ConnectionState.Closed)
                        dbConnection.Close();
                }

                if (dbCommand != null)
                    dbCommand.Parameters.Clear();
            }
        }

        /// <summary>
        ///     Este método executa instruções SQL na base de dados e retorna uma coleção de registros.
        /// </summary>
        /// <returns>
        ///     Retorna um objeto do tipo <see cref="DataTable"/>.
        /// </returns>
        public DbResult<DataTable> ExecuteDataTable(string commandText)
        {
            try
            {
                using (DataTable dtRetorno = new DataTable())
                {
                    if (!isTransaction)
                        if (dbConnection.State == ConnectionState.Closed)
                            dbConnection.Open();

                    dbCommand.CommandText = commandText;
                    dbCommand.CommandType = CommandType.StoredProcedure;

                    using (IDataReader drLeitorAuxiliar = dbCommand.ExecuteReader())
                    {
                        if (drLeitorAuxiliar != null)
                        {
                            dtRetorno.Load(drLeitorAuxiliar);
                            drLeitorAuxiliar.Close();
                        }
                    }

                    return DbResultFactory.CreateDbResult(dtRetorno);
                }
            }
            catch (Exception exception)
            {
                return DbResultFactory.CreateDbErrorResult<DataTable>(exception);
            }
            finally
            {
                if (!isTransaction)
                {
                    if (dbConnection.State != ConnectionState.Closed)
                        dbConnection.Close();
                }

                if (dbCommand != null)
                    dbCommand.Parameters.Clear();
            }
        }

        /// <summary>
        ///     Este método executa instruções SQL na base de dados e retorna uma coleção de registros.
        /// </summary>
        /// <returns>
        ///     Retorna um objeto do tipo <see cref="DataTable"/>.
        /// </returns>
        public DbResult<IDataReader> ExecuteDataReader(string commandText)
        {
            try
            {
                if (!isTransaction)
                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                dbCommand.CommandText = commandText;
                dbCommand.CommandType = CommandType.StoredProcedure;

                dataReader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

                return DbResultFactory.CreateDbResult(dataReader);
            }
            catch (Exception exception)
            {
                return DbResultFactory.CreateDbErrorResult<IDataReader>(exception);
            }
            finally
            {
                if (dbCommand != null)
                    dbCommand.Parameters.Clear();
            }
        }

        #endregion
    }
}