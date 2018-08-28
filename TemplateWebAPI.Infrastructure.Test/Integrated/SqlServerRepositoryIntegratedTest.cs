using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using TemplateWebAPI.Infrastructure.Repositories;
using TemplateWebAPI.Infrastructure.Repositories.Contracts;

namespace TemplateWebAPI.Infrastructure.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SqlServerRepositoryIntegratedTest
    {
        [TestMethod]
        public void ExecuteScalar_Sucesso()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            var result = repository.ExecuteScalar("SPFU1_TEST_RETORNA_VALOR_ESCALAR");
            repository.Dispose();

            //Assert
            Assert.IsInstanceOfType(result.Value, typeof(object));
            Assert.IsFalse(result.HasError);
        }

        [TestMethod]
        public void ExecuteScalar_Erro()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            var result = repository.ExecuteScalar("NOME DA PROCEDURE ERRADO");
            repository.Dispose();

            //Assert
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.HasError);
            Assert.IsNotInstanceOfType(result.Value, typeof(object));
        }

        [TestMethod]
        public void ExecuteNonQuery_Sucesso()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            int result = Convert.ToInt32(repository.ExecuteScalar("SPFU1_TEST_RETORNA_VALOR_ESCALAR").Value) + 1;

            repository.AddParameter("@COD_NRMA_CTBL", result, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@NOM_NRMA_CTBL", "TEST_" + DateTime.Now.ToString(), ParameterDirection.Input, SqlDbType.VarChar);
            
            var result2 = repository.ExecuteNonQuery("SPFU1_TEST_INSERT");
            repository.Dispose();

            //Assert
            Assert.IsInstanceOfType(result2.Value, typeof(int));
            Assert.IsFalse(result2.HasError);
        }

        [TestMethod]
        public void ExecuteNonQuery_Erro()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            int result = Convert.ToInt32(repository.ExecuteScalar("SPFU1_TEST_RETORNA_VALOR_ESCALAR").Value) + 1;

            repository.AddParameter("@COD_NRMA_CTBL", result, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@NOM_NRMA_CTBL", "TEST_" + DateTime.Now.ToString(), ParameterDirection.Input, SqlDbType.VarChar);

            var result2 = repository.ExecuteNonQuery("NOME DA PROCEDURE ERRADO");
            repository.Dispose();

            //Assert
            Assert.IsInstanceOfType(result2.Value, typeof(int));
            Assert.IsTrue(result2.HasError);
        }

        [TestMethod]
        public void ExecuteDataTable_Sucesso()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            var result = repository.ExecuteDataTable("SPFU1_TBFU1744_NRMA_CTBL_SELECT");
            repository.Dispose();

            //Assert
            Assert.IsTrue(result.Message == string.Empty);
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(DataTable));
        }

        [TestMethod]
        public void ExecuteDataTable_Erro()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            var result = repository.ExecuteDataTable("NOME DA PROCEDURE ERRADO");
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Message));
            Assert.IsTrue(result.HasError);
            Assert.IsNotInstanceOfType(result.Value, typeof(DataTable));
        }

        [TestMethod]
        public void ExecuteDataReader_Sucesso()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            var result = repository.ExecuteDataReader("SPFU1_TBFU1744_NRMA_CTBL_SELECT");
            repository.Dispose();

            //Assert
            Assert.IsTrue(result.Message == string.Empty);
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(IDataReader));
        }

        [TestMethod]
        public void ExecuteDataReader_Erro()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            var result = repository.ExecuteDataReader("NOME DA PROCEDURE ERRADO");
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Message));
            Assert.IsTrue(result.HasError);
            Assert.IsNotInstanceOfType(result.Value, typeof(IDataReader));
        }

        [TestMethod]
        public void ExecuteDataTable_Transaction_Sucesso()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            repository.BeginTransaction(IsolationLevel.ReadCommitted);
            var result = repository.ExecuteDataTable("SPFU1_TBFU1744_NRMA_CTBL_SELECT");
            repository.EndTransaction(true);
            repository.Dispose();

            //Assert
            Assert.IsTrue(result.Message == string.Empty);
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(DataTable));
        }

        [TestMethod]
        public void ExecuteDataTable_Transaction_Erro()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            repository.BeginTransaction(IsolationLevel.ReadCommitted);
            var result = repository.ExecuteDataTable("NOME DA PROCEDURE ERRADO");
            repository.EndTransaction(true);
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Message));
            Assert.IsTrue(result.HasError);
            Assert.IsNotInstanceOfType(result.Value, typeof(DataTable));
        }

        [TestMethod]
        public void ExecuteNonQuery_Transaction_Sucesso()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            repository.BeginTransaction(IsolationLevel.ReadCommitted);

            int result = Convert.ToInt32(repository.ExecuteScalar("SPFU1_TEST_RETORNA_VALOR_ESCALAR").Value) + 1;

            repository.AddParameter("@COD_NRMA_CTBL", result, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@NOM_NRMA_CTBL", "TEST_" + DateTime.Now.ToString(), ParameterDirection.Input, SqlDbType.VarChar);

            var result2 = repository.ExecuteNonQuery("SPFU1_TEST_INSERT");

            repository.EndTransaction(true);
            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result2.Message));
            Assert.IsFalse(result2.HasError);
            Assert.IsInstanceOfType(result2.Value, typeof(int));
        }

        [TestMethod]
        public void ExecuteNonQuery_Transaction_Erro()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            repository.BeginTransaction(IsolationLevel.ReadCommitted);

            int result = Convert.ToInt32(repository.ExecuteScalar("SPFU1_TEST_RETORNA_VALOR_ESCALAR").Value) + 1;

            repository.AddParameter("@COD_NRMA_CTBL", result, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@NOM_NRMA_CTBL", "TEST_" + DateTime.Now.ToString(), ParameterDirection.Input, SqlDbType.VarChar);

            var result2 = repository.ExecuteNonQuery("NOME DA PROCEDURE ERRADO");

            repository.EndTransaction(false);
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result2.Message));
            Assert.IsTrue(result2.HasError);
            Assert.IsInstanceOfType(result2.Value, typeof(int));
        }

        [TestMethod]
        public void BeginTransaction_Connection_Opened_Erro()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            repository.BeginTransaction(IsolationLevel.ReadCommitted);

            var result = repository.BeginTransaction(IsolationLevel.ReadCommitted);

            repository.EndTransaction(false);
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Message));
            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(bool));
        }

        [TestMethod]
        public void BeginTransaction_Rollback_Erro()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository("CONNECTION STRING ERRADA");

            //Act
            var result = repository.BeginTransaction(IsolationLevel.ReadCommitted);
            repository.EndTransaction(false);
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Message));
            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(bool));
        }

        [TestMethod]
        public void EndTransaction_Commit_Sucesso()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            repository.BeginTransaction(IsolationLevel.ReadCommitted);

            var result = repository.ExecuteDataReader("SPFU1_TBFU1744_NRMA_CTBL_SELECT");

            repository.EndTransaction(true);
            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(IDataReader));
        }

        [TestMethod]
        public void EndTransaction_Rollback_Sucesso()
        {
            //Arrange
            ISqlServerRepository repository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Sql-ConnString-Token"].ConnectionString);

            //Act
            repository.BeginTransaction(IsolationLevel.ReadCommitted);

            var result = repository.ExecuteDataReader("SPFU1_TBFU1744_NRMA_CTBL_SELECT");

            repository.EndTransaction(false);
            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(IDataReader));
        }
    }
}