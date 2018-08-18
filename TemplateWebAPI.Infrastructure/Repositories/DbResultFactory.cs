using System;

namespace TemplateWebAPI.Infrastructure.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///     Nome: Onofre Antonio Juvencio Jr.
    ///     Data: 15/07/2018
    /// </remarks>
    public static class DbResultFactory
    {
        public static DbResult<T> CreateDbResult<T>(T value)
        {
            return new DbResult<T>()
            {
                Value = value,
                HasError = false,
                Message = string.Empty
            };
        }

        public static DbResult<T> CreateDbErrorResult<T>(Exception exception)
        {
            return new DbResult<T>()
            {
                Value = default(T),
                HasError = true,
                Message = exception.Message
            };
        }
    }
}