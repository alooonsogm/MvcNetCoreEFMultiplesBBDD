using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;

#region VISTAS/PROCEDURE
//SQL SERVER
//CREATE view V_EMPLEADOS
//as
//	SELECT EMP.EMP_NO AS IDEMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
//    DEPT.DEPT_NO AS IDDEPARTAMENTO, DEPT.DNOMBRE AS DEPARTAMENTO, DEPT.LOC AS LOCALIDAD
//	FROM EMP INNER JOIN DEPT ON EMP.DEPT_NO=DEPT.DEPT_NO;
//go

//SELECT * FROM V_EMPLEADOS

//ORACLE
//create or replace VIEW V_EMPLEADOS
//as
//SELECT EMP.EMP_NO AS IDEMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
//DEPT.DEPT_NO AS IDDEPARTAMENTO, DEPT.DNOMBRE AS DEPARTAMENTO, DEPT.LOC AS LOCALIDAD
//FROM EMP INNER JOIN DEPT ON EMP.DEPT_NO=DEPT.DEPT_NO

//SELECT * from V_EMPLEADOS;
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<EmpleadosVista>> GetVistaEmpleadosAsync()
        {
            var consulta = from datos in this.context.Empleados select datos;
            return await consulta.ToListAsync();
        }
        public async Task<EmpleadosVista> FindVistaEmpleadosAsync(int idEmpleado)
        {
            var consulta = from datos in this.context.Empleados where datos.IdEmpleado == idEmpleado select datos;
            return await consulta.FirstOrDefaultAsync();
        }
    }
}
