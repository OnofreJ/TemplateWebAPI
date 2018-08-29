using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TemplateWebAPI.Infrastructure.Repositories
{
    /// <summary>
    ///     Esta classe é um mapeador de tabela-para-objeto DTOs (Data Transfer Objects), 
    ///     transformando uma tabela (ou um resultset qualquer) em outro tipo.
    /// </summary>
    /// <remarks>
    ///     Nome: Onofre Antonio Juvencio Jr.
    ///     Data: 15/07/2018
    /// </remarks>
    /// <typeparam name="TEntity">Tipo da entidade de negócio padrão (DTO) a ser retornado.</typeparam>
    public class Mapper<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        ///     Método que retorna uma lista com objetos DTOs (Data Transfer Objects), dada uma tabela (resultset).
        /// </summary>
        /// <param name="table">Tabela a ser mapeada para objeto DTO (Data Transfer Object).</param>
        /// <param name="inherit">Indica se as classes bases deverão ser incluídas na lista de attributes.</param>
        /// <returns>Retorna uma objeto do tipo <see cref="IEnumerable<TEntity>"/>.</returns>
        public IEnumerable<TEntity> Map(DataTable table, bool inherit)
        {
            Type entityType = typeof(TEntity);
            Lazy<List<TEntity>> list = new Lazy<List<TEntity>>();
            Mapping[] attributes = entityType.GetCustomAttributes(typeof(Mapping), inherit) as Mapping[]; //Verificar se pode usar herança.

            for (int counter = table.Rows.Count - 1; counter >= 0; counter--)
            {
                DataRow row = table.Rows[counter];
                var entity = new TEntity();

                foreach (var attribute in attributes)
                {
                    var property = entityType.GetProperty(attribute.PropertyName);

                    if (property != null && property.CanWrite)
                    {
                        if (!row.Table.Columns.Contains(attribute.ColumnName))
                        {
                            var propertyValue = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
                            property.SetValue(entity, propertyValue);
                        }
                        else
                        {
                            var propertyValue = row[attribute.ColumnName] == DBNull.Value ? null : row[attribute.ColumnName];
                            property.SetValue(entity, propertyValue, null);
                        }
                    }
                }

                list.Value.Add(entity);
            }

            return list.Value.ToList();
        }
    }
}