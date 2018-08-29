using System.Data;
using TemplateWebAPI.API.Models;
using TemplateWebAPI.Infrastructure.Repositories;

namespace TemplateWebAPI.API.Repositories
{
    public interface IMotivoAjusteRepository
    {
        DbResult<int> Alterar(MotivoAjuste motivoAjuste);
        DbResult<DataTable> Consultar(MotivoAjuste motivoAjuste);
        DbResult<int> Excluir(MotivoAjuste motivoAjuste);
        DbResult<int> Incluir(MotivoAjuste motivoAjuste);
    }
}