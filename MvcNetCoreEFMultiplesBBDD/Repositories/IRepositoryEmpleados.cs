using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public interface IRepositoryEmpleados
    {
        Task<List<EmpleadosVista>> GetVistaEmpleadosAsync();
        Task<EmpleadosVista> FindVistaEmpleadosAsync(int idEmpleado);
        Task<int> InsertEmpleadosAsync(string apellido, string oficio, int dir, int salario, int comision, string nombreDepart);
    }
}
