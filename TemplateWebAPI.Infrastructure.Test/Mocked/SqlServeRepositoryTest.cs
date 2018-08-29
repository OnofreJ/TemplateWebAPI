using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using TemplateWebAPI.Infrastructure.Repositories;
using TemplateWebAPI.Infrastructure.Repositories.Contracts;

namespace TemplateWebAPI.Infrastructure.Test.Mocked
{
    [ExcludeFromCodeCoverage]
    public class MockIDbTransaction : IDbTransaction
    {
        public IDbConnection Connection { get; set; }

        public IsolationLevel IsolationLevel { get; set; }

        public virtual void Commit() { }

        public virtual void Rollback() { }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    [ExcludeFromCodeCoverage]
    public class MockIDbConnection : IDbConnection
    {
        public string ConnectionString { get; set; }

        public int ConnectionTimeout { get; set; }

        public string Database { get; set; }
        
        public ConnectionState State { get; set; }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public virtual IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public virtual void Close() { }

        public void CustomClose()
        {
            State = ConnectionState.Closed;
        }

        public void CustomOpen()
        {
            State = ConnectionState.Open;
        }
        public virtual IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public virtual void Open() { }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {   
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
    }
    
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SqlServeRepositoryTest
    {
        public Mock<IDataReader> MockDataReader()
        {
            const string primeiraColuna = "primeiraColuna";
            const string segundaColuna = "segundaColuna";
            const string primeiroValor = "primeiroValor";
            const string segundoValor = "segundoValor";
            var iDataReader = new Mock<IDataReader>();

            iDataReader.Setup(mock => mock.FieldCount).Returns(2);
            iDataReader.Setup(mock => mock.GetName(0)).Returns(primeiraColuna);
            iDataReader.Setup(mock => mock.GetName(1)).Returns(segundaColuna);

            iDataReader.Setup(mock => mock.GetFieldType(0)).Returns(typeof(string));
            iDataReader.Setup(mock => mock.GetFieldType(1)).Returns(typeof(string));

            iDataReader.Setup(mock => mock.GetOrdinal("primeiraColuna")).Returns(0);
            iDataReader.Setup(mock => mock.GetValue(0)).Returns(primeiroValor);
            iDataReader.Setup(mock => mock.GetValue(1)).Returns(segundoValor);

            iDataReader.SetupSequence(mock => mock.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            return iDataReader;
        }

        [TestMethod]
        public void ExecuteDataReader_Sucesso()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<IDbConnection> iDbConnection = new Mock<IDbConnection>();            

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.ExecuteReader(CommandBehavior.CloseConnection)).Returns(MockDataReader().Object);
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var result = repository.ExecuteDataReader("PROCEDURE");

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(IDataReader));
        }

        [TestMethod]
        public void ExecuteDataReader_Erro()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<IDbConnection> iDbConnection = new Mock<IDbConnection>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.ExecuteReader(CommandBehavior.CloseConnection)).Throws(new Exception("Erro!"));
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var result = repository.ExecuteDataReader("PROCEDURE");

            //Assert
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.HasError);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public void ExecuteDataTable_Sucesso()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<IDbConnection> iDbConnection = new Mock<IDbConnection>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.ExecuteReader()).Returns(MockDataReader().Object);
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var result = repository.ExecuteDataTable("PROCEDURE");

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(DataTable));
        }

        [TestMethod]
        public void ExecuteDataTable_Erro()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<IDbConnection> iDbConnection = new Mock<IDbConnection>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.ExecuteReader()).Throws(new Exception("Erro!"));
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var result = repository.ExecuteDataTable("PROCEDURE");

            //Assert
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.HasError);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public void ExecuteNonQuery_Sucesso()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.ExecuteNonQuery()).Returns(1);
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var result = repository.ExecuteNonQuery("PROCEDURE");
            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(int));
        }

        [TestMethod]
        public void ExecuteNonQuery_Erro()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.ExecuteNonQuery()).Throws(new Exception("Erro!"));
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var result = repository.ExecuteNonQuery("PROCEDURE");
            repository.Dispose();

            //Assert
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.HasError);
            Assert.IsTrue(result.Value == 0);
        }

        [TestMethod]
        public void ExecuteScalar_Sucesso()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.ExecuteScalar()).Returns(1);
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var result = repository.ExecuteScalar("PROCEDURE");
            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(int));
        }

        [TestMethod]
        public void ExecuteScalar_Erro()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<IDbConnection> iDbConnection = new Mock<IDbConnection>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.ExecuteScalar()).Throws(new Exception("Erro!"));
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var result = repository.ExecuteScalar("PROCEDURE");

            //Assert
            Assert.IsNotNull(result.Message);
            Assert.IsTrue(result.HasError);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public void BeginTransaction_Sucesso()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            Mock<IDbTransaction> iDbTransaction = new Mock<IDbTransaction>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbCommand.Setup(mock => mock.ExecuteReader()).Returns(MockDataReader().Object);

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);
            iDbConnection.Setup(mock => mock.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(iDbTransaction.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);

            //Act
            var result = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            repository.ExecuteDataReader("PROCEDURE");
            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));
            Assert.IsFalse(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(bool));
        }

        [TestMethod]
        public void BeginTransaction_Check_Opened_Connection_Erro()
        {
            //Arrange
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            Mock<IDbTransaction> iDbTransaction = new Mock<IDbTransaction>();

            iDbConnection.Object.CustomOpen();
            iDbConnection.Setup(mock => mock.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(iDbTransaction.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);

            //Act
            var result = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Message));
            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(bool));
        }

        [TestMethod]
        public void BeginTransaction_Open_Connection_Erro()
        {
            //Arrange
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            iDbConnection.Setup(mock => mock.Open()).Throws(new Exception("Erro!"));

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);

            //Act
            var result = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Message));
            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(bool));
        }

        [TestMethod]
        public void BeginTransaction_BeginTransaction_Erro()
        {
            //Arrange
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.BeginTransaction(IsolationLevel.ReadUncommitted)).Throws(new Exception("Erro!"));

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);

            //Act
            var result = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Message));
            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(bool));
        }

        [TestMethod]
        public void BeginTransaction_Erro_Transaction_Rollback_Erro()
        {
            //Arrange
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            Mock<MockIDbTransaction> iDbTransaction = new Mock<MockIDbTransaction>();
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            
            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbCommand.SetupGet(mock => mock.Transaction).Throws(new Exception("Erro!"));

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(iDbTransaction.Object);

            iDbTransaction.Setup(mock => mock.Rollback()).Throws(new Exception("Erro!"));

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);

            //Act
            var result = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            repository.Dispose();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Message));
            Assert.IsTrue(result.HasError);
            Assert.IsInstanceOfType(result.Value, typeof(bool));
        }

        [TestMethod]
        public void BeginTransaction_Erro_Connection_Close_Erro()
        {
            //Arrange
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            Mock<MockIDbTransaction> iDbTransaction = new Mock<MockIDbTransaction>();
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbCommand.SetupGet(mock => mock.Transaction).Throws(new Exception("Erro!"));

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(iDbTransaction.Object);
            iDbConnection.Setup(mock => mock.Close()).Throws(new Exception("Erro!"));

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);

            //Act
            var result = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            iDbConnection.Object.CustomClose();
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
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            Mock<IDbTransaction> iDbTransaction = new Mock<IDbTransaction>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbCommand.Setup(mock => mock.ExecuteReader(CommandBehavior.CloseConnection)).Returns(MockDataReader().Object);

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);
            iDbConnection.Setup(mock => mock.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(iDbTransaction.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var resultBeginTransaction = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            var resultExecuteDataReader = repository.ExecuteDataReader("PROCEDURE");
            var resultEndTransaction = repository.EndTransaction(true);

            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(resultBeginTransaction.Message));
            Assert.IsTrue(string.IsNullOrEmpty(resultEndTransaction.Message));
            Assert.IsTrue(string.IsNullOrEmpty(resultExecuteDataReader.Message)); 

            Assert.IsFalse(resultBeginTransaction.HasError);
            Assert.IsFalse(resultEndTransaction.HasError);
            Assert.IsFalse(resultExecuteDataReader.HasError);

            Assert.IsInstanceOfType(resultBeginTransaction.Value, typeof(bool));
            Assert.IsInstanceOfType(resultEndTransaction.Value, typeof(bool));
            Assert.IsInstanceOfType(resultExecuteDataReader.Value, typeof(IDataReader));
        }

        [TestMethod]
        public void EndTransaction_Rollback_Sucesso()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            Mock<IDbTransaction> iDbTransaction = new Mock<IDbTransaction>();

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbCommand.Setup(mock => mock.ExecuteReader(CommandBehavior.CloseConnection)).Returns(MockDataReader().Object);

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);
            iDbConnection.Setup(mock => mock.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(iDbTransaction.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var resultBeginTransaction = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            var resultExecuteDataReader = repository.ExecuteDataReader("PROCEDURE");
            var resultEndTransaction = repository.EndTransaction(false);

            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(resultBeginTransaction.Message));
            Assert.IsTrue(string.IsNullOrEmpty(resultEndTransaction.Message));
            Assert.IsTrue(string.IsNullOrEmpty(resultExecuteDataReader.Message));

            Assert.IsFalse(resultBeginTransaction.HasError);
            Assert.IsFalse(resultEndTransaction.HasError);
            Assert.IsFalse(resultExecuteDataReader.HasError);

            Assert.IsInstanceOfType(resultBeginTransaction.Value, typeof(bool));
            Assert.IsInstanceOfType(resultEndTransaction.Value, typeof(bool));
            Assert.IsInstanceOfType(resultExecuteDataReader.Value, typeof(IDataReader));
        }

        [TestMethod]
        public void EndTransaction_Commit_Erro()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            Mock<IDbTransaction> iDbTransaction = new Mock<IDbTransaction>();

            iDbTransaction.Setup(mock => mock.Commit()).Throws(new Exception("Erro!"));

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbCommand.Setup(mock => mock.ExecuteReader(CommandBehavior.CloseConnection)).Returns(MockDataReader().Object);

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);
            iDbConnection.Setup(mock => mock.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(iDbTransaction.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var resultBeginTransaction = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            var resultExecuteDataReader = repository.ExecuteDataReader("PROCEDURE");
            var resultEndTransaction = repository.EndTransaction(true);

            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(resultBeginTransaction.Message));
            Assert.IsTrue(string.IsNullOrEmpty(resultExecuteDataReader.Message));
            Assert.IsFalse(string.IsNullOrEmpty(resultEndTransaction.Message));

            Assert.IsFalse(resultBeginTransaction.HasError);
            Assert.IsFalse(resultExecuteDataReader.HasError);
            Assert.IsTrue(resultEndTransaction.HasError);

            Assert.IsInstanceOfType(resultBeginTransaction.Value, typeof(bool));
            Assert.IsInstanceOfType(resultEndTransaction.Value, typeof(bool));
            Assert.IsInstanceOfType(resultExecuteDataReader.Value, typeof(IDataReader));
        }

        [TestMethod]
        public void EndTransaction_Commit_Rollback_Erro()
        {
            //Arrange
            Mock<IDbCommand> iDbCommand = new Mock<IDbCommand>();
            Mock<MockIDbConnection> iDbConnection = new Mock<MockIDbConnection>();
            Mock<IDbTransaction> iDbTransaction = new Mock<IDbTransaction>();

            iDbTransaction.Setup(mock => mock.Commit()).Throws(new Exception("Erro!"));
            iDbTransaction.Setup(mock => mock.Rollback()).Throws(new Exception("Erro!"));

            iDbCommand.Object.Connection = iDbConnection.Object;
            iDbCommand.Setup(mock => mock.Parameters).Returns(new SqlCommand().Parameters);
            iDbCommand.Setup(mock => mock.ExecuteReader(CommandBehavior.CloseConnection)).Returns(MockDataReader().Object);

            iDbConnection.Setup(mock => mock.Open()).Callback(() => iDbConnection.Object.CustomOpen());
            iDbConnection.Setup(mock => mock.CreateCommand()).Returns(iDbCommand.Object);
            iDbConnection.Setup(mock => mock.BeginTransaction(IsolationLevel.ReadUncommitted)).Returns(iDbTransaction.Object);

            ISqlServerRepository repository = new SqlServerRepository(iDbConnection.Object);
            repository.AddParameter("@1", null, ParameterDirection.Input, SqlDbType.Int);
            repository.AddParameter("@2", null, ParameterDirection.Input, SqlDbType.VarChar);

            //Act
            var resultBeginTransaction = repository.BeginTransaction(IsolationLevel.ReadUncommitted);
            var resultExecuteDataReader = repository.ExecuteDataReader("PROCEDURE");
            var resultEndTransaction = repository.EndTransaction(true);

            repository.Dispose();

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(resultBeginTransaction.Message));
            Assert.IsTrue(string.IsNullOrEmpty(resultExecuteDataReader.Message));
            Assert.IsFalse(string.IsNullOrEmpty(resultEndTransaction.Message));

            Assert.IsFalse(resultBeginTransaction.HasError);
            Assert.IsFalse(resultExecuteDataReader.HasError);
            Assert.IsTrue(resultEndTransaction.HasError);

            Assert.IsInstanceOfType(resultBeginTransaction.Value, typeof(bool));
            Assert.IsInstanceOfType(resultEndTransaction.Value, typeof(bool));
            Assert.IsInstanceOfType(resultExecuteDataReader.Value, typeof(IDataReader));
        }
    }
}