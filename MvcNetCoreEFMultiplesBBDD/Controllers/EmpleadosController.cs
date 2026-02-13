using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;
using System.Threading.Tasks;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<EmpleadosVista> empleados = await this.repo.GetVistaEmpleadosAsync();
            return View(empleados);
        }
        public async Task<IActionResult> Details(int id)
        {
            EmpleadosVista empleado = await this.repo.FindVistaEmpleadosAsync(id);
            return View(empleado);
        }
    }
}
