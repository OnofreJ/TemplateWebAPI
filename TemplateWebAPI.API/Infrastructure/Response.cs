using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TemplateWebAPI.API.Infrastructure
{
    public sealed class Response<T>
    {
        [ExcludeFromCodeCoverage]
        public Response() { }

        public Response(bool hasError, string message, IEnumerable<T> modelList, T model)
        {
            HasError = hasError;
            Message = message;
            ModelList = modelList;
            Model = model;
        }

        public bool HasError { get; set; }

        public string Message { get; set; }

        public IEnumerable<T> ModelList { get; set; }

        public T Model { get; set; }
    }
}