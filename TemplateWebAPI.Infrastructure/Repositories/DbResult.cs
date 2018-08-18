namespace TemplateWebAPI.Infrastructure.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbResult<T>
    {
        public bool HasError { get; set; }

        public string Message { get; set; }

        public T Value { get; set; }
    }
}