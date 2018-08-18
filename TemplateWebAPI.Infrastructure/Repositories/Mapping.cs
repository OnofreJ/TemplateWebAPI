using System;

namespace TemplateWebAPI.Infrastructure.Repositories
{
    /// <summary>
    ///     Classe que associa uma coluna de uma tabela do banco de dados (resultset) com uma 
    ///     propriedade de uma classe de negócio (DTO - Data Transfer Object).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class Mapping : Attribute
    {
        /// <summary>
        ///     Propriedade que recebe/retorna o nome de uma coluna de uma tabela.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     Propriedade que recebe/retorna o nome de uma propriedade de uma 
        ///     determinada classe de negócio (DTO - Data Transfer Object).
        /// </summary>
        public string PropertyName { get; set; }
    }
}