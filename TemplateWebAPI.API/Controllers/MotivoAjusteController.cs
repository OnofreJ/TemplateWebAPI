using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Web.Http;
using TemplateWebAPI.API.Models;
using TemplateWebAPI.API.Repositories;
using TemplateWebAPI.API.Services;

namespace Itau.Ajustes.API.Controllers
{
    [Route("api/ajustes/")]
    public class MotivoAjusteController : ApiController
    {
        private readonly IMotivoAjusteRepository motivoAjusteRepository;
        private readonly IMotivoAjusteService motivoAjusteService;

        [ExcludeFromCodeCoverage]
        public MotivoAjusteController()
        {
            motivoAjusteRepository = new MotivoAjusteRepository();
            motivoAjusteService = new MotivoAjusteService(motivoAjusteRepository);
        }

        public MotivoAjusteController(IMotivoAjusteService motivoAjusteService, IMotivoAjusteRepository motivoAjusteRepository)
        {
            this.motivoAjusteRepository = motivoAjusteRepository;
            this.motivoAjusteService = motivoAjusteService;
        }
        
        [HttpGet]
        public IHttpActionResult Consultar()
        {
            var retorno = motivoAjusteService.Consultar(new MotivoAjuste()
            {
                NumeroMotivoAjuste = null,
                CodigoSituacao = null,
                NomeMotivoAjuste = null,
                CodigoGrupoResponsavel = null,
                IndicadorPeriodicidade = null,
                Racf = null
            });

            if (retorno.HasError)
                return Content(HttpStatusCode.BadRequest, retorno);
            else
                return Ok(retorno);
        }

        [HttpGet]
        public IHttpActionResult Consultar(string numeroMotivoAjuste,
                string codigoSituacao,
                string nomeMotivoAjuste,
                string codigoGrupoResponsavel,
                string indicadorPeriodicidade,
                string racf)
        {
            var retorno = motivoAjusteService.Consultar(new MotivoAjuste()
            {
                NumeroMotivoAjuste = string.IsNullOrEmpty(numeroMotivoAjuste) ? (int?)null : Convert.ToInt32(numeroMotivoAjuste),
                CodigoSituacao = string.IsNullOrEmpty(codigoSituacao) ? (byte?)null : Convert.ToByte(codigoSituacao),
                NomeMotivoAjuste = nomeMotivoAjuste,
                CodigoGrupoResponsavel = string.IsNullOrEmpty(codigoGrupoResponsavel) ? (byte?)null : Convert.ToByte(codigoGrupoResponsavel),
                IndicadorPeriodicidade = indicadorPeriodicidade,
                Racf = racf
            });

            if (retorno.HasError)
                return Content(HttpStatusCode.BadRequest, retorno);
            else
                return Ok(retorno);            
        }

        [HttpPut]
        public IHttpActionResult Alterar([FromBody]MotivoAjuste motivoAjuste)
        {
            var retorno = motivoAjusteService.Alterar(motivoAjuste);

            if (retorno.HasError)
                return Content(HttpStatusCode.BadRequest, retorno);
            else
                return Ok(retorno);
        }

        [HttpPost]
        public IHttpActionResult Incluir([FromBody]MotivoAjuste motivoAjuste)
        {
            var retorno = motivoAjusteService.Incluir(motivoAjuste);

            if (retorno.HasError)
                return Content(HttpStatusCode.BadRequest, retorno);
            else
                return Ok(retorno);
        }

        [HttpDelete]
        public IHttpActionResult Excluir(int numeroMotivoAjuste)
        {
            var retorno = motivoAjusteService.Excluir(new MotivoAjuste()
            {
                NumeroMotivoAjuste = numeroMotivoAjuste
            });

            if (retorno.HasError)
                return Content(HttpStatusCode.BadRequest, retorno);
            else if (retorno.Model == 0)
                return Content(HttpStatusCode.NotFound, retorno);
            else
                return Ok(retorno);
        }
    }
}
