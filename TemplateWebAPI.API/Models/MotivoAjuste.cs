using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateWebAPI.API.Models
{
    public class MotivoAjuste
    {
        internal object Racf;

        public object NumeroMotivoAjuste { get; internal set; }
        public object CodigoSituacao { get; internal set; }
        public object NomeMotivoAjuste { get; internal set; }
        public object CodigoGrupoResponsavel { get; internal set; }
        public object IndicadorPeriodicidade { get; internal set; }
        public object DataInicioVigenciaAjuste { get; internal set; }
        public object DataFimVigenciaAjuste { get; internal set; }
        public object DiaUtilExecucaoAjuste { get; internal set; }
        public object DescricaoMotivoAjuste { get; internal set; }
        public object CodigoUsuarioManutencao { get; internal set; }
    }
}