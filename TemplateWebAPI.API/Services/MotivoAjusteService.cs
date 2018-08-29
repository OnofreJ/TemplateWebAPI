using System.Linq;
using TemplateWebAPI.API.Infrastructure;
using TemplateWebAPI.API.Models;
using TemplateWebAPI.API.Repositories;
using TemplateWebAPI.Infrastructure.Repositories;

namespace TemplateWebAPI.API.Services
{
    public sealed class MotivoAjusteService : IMotivoAjusteService
    {
        readonly IMotivoAjusteRepository motivoAjusteRepository;

        public MotivoAjusteService(IMotivoAjusteRepository motivoAjusteRepository)
        {
            this.motivoAjusteRepository = motivoAjusteRepository;
        }

        public Response<MotivoAjuste> Consultar(MotivoAjuste motivoAjuste)
        {
            var retorno = motivoAjusteRepository.Consultar(motivoAjuste);

            if (retorno.HasError)
                return new Response<MotivoAjuste>(true, retorno.Message, null, null);
            else
                return new Response<MotivoAjuste>(false, string.Empty, new Mapper<MotivoAjuste>().Map(retorno.Value, false).ToList(), null);
        }

        public Response<int> Alterar(MotivoAjuste motivoAjuste)
        {
            var retorno = motivoAjusteRepository.Alterar(motivoAjuste);

            if (retorno.HasError)
                return new Response<int>(true, retorno.Message, null, int.MinValue);
            else
                return new Response<int>(false, null, null, retorno.Value);
        }

        public Response<int> Excluir(MotivoAjuste motivoAjuste)
        {
            var retorno = motivoAjusteRepository.Excluir(motivoAjuste);

            if (retorno.HasError)
                return new Response<int>(true, retorno.Message, null, int.MinValue);
            else
                return new Response<int>(false, null, null, retorno.Value);
        }

        public Response<int> Incluir(MotivoAjuste motivoAjuste)
        {
            var retorno = motivoAjusteRepository.Incluir(motivoAjuste);

            if (retorno.HasError)
                return new Response<int>(true, retorno.Message, null, int.MinValue);
            else
                return new Response<int>(false, null, null, retorno.Value);
        }
    }
}