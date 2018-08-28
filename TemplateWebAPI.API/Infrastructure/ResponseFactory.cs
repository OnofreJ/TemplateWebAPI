using System.Diagnostics.CodeAnalysis;

namespace TemplateWebAPI.API.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class ResponseFactory
    {
        public static Response<T> CreateResponse<T>(T value)
        {
            return new Response<T>();
        }

        public static Response<T> CreateErrorResponse<T>(T value)
        {
            return new Response<T>();
        }
    }
}