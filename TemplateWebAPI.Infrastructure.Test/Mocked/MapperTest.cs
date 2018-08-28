using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TemplateWebAPI.Infrastructure.Repositories;

namespace TemplateWebAPI.Infrastructure.Test
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class MapperTest
    {
        [TestMethod]
        public void Map_Sucesso()
        {
            //Arrange
            DbResult<DataTable> T = DbResultFactory.CreateDbResult(BuildTableMotivoAjuste());
            Mapper<NormaContabil> map = new Mapper<NormaContabil>();

            //Act
            List<NormaContabil> normas = map.Map(T.Value, false).ToList();

            //Assert
            Assert.IsInstanceOfType(normas, typeof(List<NormaContabil>));
            Assert.IsTrue(normas.Count > 0);
        }

        [Serializable]
        [Mapping(ColumnName = "COD_NRMA_CTBL", PropertyName = "Codigo")]
        [Mapping(ColumnName = "NOM_NRMA_CTBL", PropertyName = "Nome")]
        [Mapping(ColumnName = "COD_SITU_AJUS_GEEN", PropertyName = "CodigoSituacao")]
        [Mapping(ColumnName = "", PropertyName = "PropriedadeQueNaoExiste")]
        [Mapping(ColumnName = "ColunaQueNaoExiste", PropertyName = "")]
        public class NormaContabil
        {
            public string Nome { get; set; }
            public int? Codigo { get; set;} 
            public int CodigoSituacao { get; set; }
            public int PropriedadeQueNaoExiste { get; set; }
        }

        [ExcludeFromCodeCoverage]
        private DataTable BuildTableMotivoAjuste()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("COD_NRMA_CTBL", typeof(int)));
            table.Columns.Add(new DataColumn("NOM_NRMA_CTBL", typeof(string)));
            table.Columns.Add(new DataColumn("COD_SITU_AJUS_GEEN", typeof(int)));

            for (int i = 0; i < 10; i++)
            {
                DataRow row = table.NewRow();
                row[0] = i + 1;
                row[1] = "NOME TESTE " + i.ToString();
                row[2] = ((i % 2) == 1) ? DBNull.Value : (object)i;

                table.Rows.Add(row);
            }

            return table;
        }
    }
}