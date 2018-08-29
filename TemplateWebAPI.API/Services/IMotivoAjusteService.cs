using TemplateWebAPI.API.Infrastructure;
using TemplateWebAPI.API.Models;

namespace TemplateWebAPI.API.Services
{
    public interface IMotivoAjusteService
    {
        Response<int> Alterar(MotivoAjuste motivoAjuste);
        Response<MotivoAjuste> Consultar(MotivoAjuste motivoAjuste);
        Response<int> Excluir(MotivoAjuste motivoAjuste);
        Response<int> Incluir(MotivoAjuste motivoAjuste);
    }
}