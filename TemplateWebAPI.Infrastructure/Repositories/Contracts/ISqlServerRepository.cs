using System;
using System.Data;

namespace TemplateWebAPI.Infrastructure.Repositories.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///     Nome: Onofre Antonio Juvencio Jr.
    ///     Data: 15/07/2018
    /// </remarks>
    public interface ISqlServerRepository : IDisposable
    {
        IDbCommand Command { get; }

        void AddParameter(string parameterName, object parameterValue, ParameterDirection parameterDirection, SqlDbType sqlDbType);

        DbResult<bool> BeginTransaction(IsolationLevel isolationLevel);

        DbResult<bool> EndTransaction(bool success);

        DbResult<IDataReader> ExecuteDataReader(string commandText);

        DbResult<DataTable> ExecuteDataTable(string commandText);

        DbResult<int> ExecuteNonQuery(string commandText);

        DbResult<object> ExecuteScalar(string commandText);
    }
}