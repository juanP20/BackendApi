using BackendAPI.Models;

namespace BackendAPI.Service.Contrato
{
    public interface IDepartamentoService
    {
        Task<List<Departamento>> GetList();

    }
}
