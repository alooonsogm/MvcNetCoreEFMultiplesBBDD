using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;
using System.Threading.Tasks;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadosController : Controller
    {
        private IRepositoryEmpleados repo;

        public EmpleadosController(IRepositoryEmpleados repo)
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

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(string apellido, string oficio, int dir, int salario, int comision, string nombreDepart)
        {
            await this.repo.InsertEmpleadosAsync(apellido, oficio, dir, salario, comision, nombreDepart);
            return RedirectToAction("Index");
        }
    }
}
