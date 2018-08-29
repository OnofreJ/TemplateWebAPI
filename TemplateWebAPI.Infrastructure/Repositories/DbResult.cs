namespace TemplateWebAPI.Infrastructure.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    ///     Nome: Onofre Antonio Juvencio Jr.
    ///     Data: 15/07/2018
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class DbResult<T>
    {
        public bool HasError { get; set; }

        public string Message { get; set; }

        public T Value { get; set; }
    }
}