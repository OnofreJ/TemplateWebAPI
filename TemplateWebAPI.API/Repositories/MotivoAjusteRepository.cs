using System.Configuration;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using TemplateWebAPI.Infrastructure.Repositories;
using TemplateWebAPI.Infrastructure.Repositories.Contracts;

namespace TemplateWebAPI.API.Repositories
{
    public sealed class MotivoAjusteRepository : IMotivoAjusteRepository
    {
        private readonly ISqlServerRepository iSqlServerRepository = null;

        [ExcludeFromCodeCoverage]
        public MotivoAjusteRepository()
        {
            iSqlServerRepository = new SqlServerRepository(ConfigurationManager.ConnectionStrings["Itau.FU1Ajuste.DES"].ConnectionString);
        }

        [ExcludeFromCodeCoverage]
        public MotivoAjusteRepository(ISqlServerRepository iSqlServerRepository)
        {
            this.iSqlServerRepository = iSqlServerRepository;
        }

        public DbResult<DataTable> Consultar(MotivoAjuste motivoAjuste)
        {
            using (iSqlServerRepository)
            {
                iSqlServerRepository.AddParameter("@P_CODMOTIVOAJUSTE", motivoAjuste.NumeroMotivoAjuste, ParameterDirection.Input, SqlDbType.Int);
                iSqlServerRepository.AddParameter("@P_CODSITUACAOAJUSTE", motivoAjuste.CodigoSituacao, ParameterDirection.Input, SqlDbType.Int);
                iSqlServerRepository.AddParameter("@P_NOMAJUSTE", motivoAjuste.NomeMotivoAjuste, ParameterDirection.Input, SqlDbType.VarChar);
                iSqlServerRepository.AddParameter("@P_CODGRUPORESP", motivoAjuste.CodigoGrupoResponsavel, ParameterDirection.Input, SqlDbType.Int);
                iSqlServerRepository.AddParameter("@P_INDPERIOD", motivoAjuste.IndicadorPeriodicidade, ParameterDirection.Input, SqlDbType.VarChar);
                iSqlServerRepository.AddParameter("@P_USERID_MANT", motivoAjuste.Racf, ParameterDirection.Input, SqlDbType.VarChar);

                return iSqlServerRepository.ExecuteDataTable("SPFU1001_LISTARMOTIVOAJUSTE");
            }
        }

        public DbResult<int> Incluir(MotivoAjuste motivoAjuste)
        {
            using (iSqlServerRepository)
            {
                iSqlServerRepository.AddParameter("@P_NOMEAJUSTE", motivoAjuste.NomeMotivoAjuste, ParameterDirection.Input, SqlDbType.VarChar);
                iSqlServerRepository.AddParameter("@P_INDPERIOD", motivoAjuste.IndicadorPeriodicidade, ParameterDirection.Input, SqlDbType.VarChar);
                iSqlServerRepository.AddParameter("@P_DTINIVIGENCIA", motivoAjuste.DataInicioVigenciaAjuste, ParameterDirection.Input, SqlDbType.DateTime);
                iSqlServerRepository.AddParameter("@P_DTFIMVIGENCIA", motivoAjuste.DataFimVigenciaAjuste, ParameterDirection.Input, SqlDbType.DateTime);
                iSqlServerRepository.AddParameter("@P_NUMDIASEXEC", motivoAjuste.DiaUtilExecucaoAjuste, ParameterDirection.Input, SqlDbType.Int);
                iSqlServerRepository.AddParameter("@P_DESCMOTIVO", motivoAjuste.DescricaoMotivoAjuste, ParameterDirection.Input, SqlDbType.Char);
                iSqlServerRepository.AddParameter("@P_CODUSUARIO", motivoAjuste.CodigoUsuarioManutencao, ParameterDirection.Input, SqlDbType.Char);
                iSqlServerRepository.AddParameter("@P_CODGRUPORESP", motivoAjuste.CodigoGrupoResponsavel, ParameterDirection.Input, SqlDbType.Int);

                return iSqlServerRepository.ExecuteNonQuery("SPFU1004_INCLUIRCADASTROAJUSTE");
            }
        }

        public DbResult<int> Alterar(MotivoAjuste motivoAjuste)
        {
            using (iSqlServerRepository)
            {
                iSqlServerRepository.AddParameter("@P_CODMOTIVOAJUSTE", motivoAjuste.NumeroMotivoAjuste, ParameterDirection.Input, SqlDbType.Int);
                iSqlServerRepository.AddParameter("@P_NOMEAJUSTE", motivoAjuste.NomeMotivoAjuste, ParameterDirection.Input, SqlDbType.VarChar);
                iSqlServerRepository.AddParameter("@P_INDPERIOD", motivoAjuste.IndicadorPeriodicidade, ParameterDirection.Input, SqlDbType.VarChar);
                iSqlServerRepository.AddParameter("@P_DTINIVIGENCIA", motivoAjuste.DataInicioVigenciaAjuste, ParameterDirection.Input, SqlDbType.DateTime);
                iSqlServerRepository.AddParameter("@P_DTFIMVIGENCIA", motivoAjuste.DataFimVigenciaAjuste, ParameterDirection.Input, SqlDbType.DateTime);
                iSqlServerRepository.AddParameter("@P_NUMDIASEXEC", motivoAjuste.DiaUtilExecucaoAjuste, ParameterDirection.Input, SqlDbType.Int);
                iSqlServerRepository.AddParameter("@P_DESCMOTIVO", motivoAjuste.DescricaoMotivoAjuste, ParameterDirection.Input, SqlDbType.Char);
                iSqlServerRepository.AddParameter("@P_CODUSUARIO", motivoAjuste.CodigoUsuarioManutencao, ParameterDirection.Input, SqlDbType.Char);
                iSqlServerRepository.AddParameter("@P_CODGRUPORESP", motivoAjuste.CodigoGrupoResponsavel, ParameterDirection.Input, SqlDbType.Int);

                return iSqlServerRepository.ExecuteNonQuery("SPFU1003_ATUALIZARCADASTROAJUSTE");
            }
        }

        public DbResult<int> Excluir(MotivoAjuste motivoAjuste)
        {
            using (iSqlServerRepository)
            {
                iSqlServerRepository.AddParameter("@P_CODMOTIVOAJUSTE", motivoAjuste.NumeroMotivoAjuste, ParameterDirection.Input, SqlDbType.Int);

                return iSqlServerRepository.ExecuteNonQuery("SPFU1035_EXCLUIRCADASTROAJUSTE");
            }
        }
    }
}