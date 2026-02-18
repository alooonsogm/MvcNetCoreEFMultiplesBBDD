using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using System.Data;

#region VISTAS/PROCEDURE
//CREATE view V_EMPLEADOS
//as
//	SELECT EMP.EMP_NO AS IDEMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
//    DEPT.DEPT_NO AS IDDEPARTAMENTO, DEPT.DNOMBRE AS DEPARTAMENTO, DEPT.LOC AS LOCALIDAD
//	FROM EMP INNER JOIN DEPT ON EMP.DEPT_NO=DEPT.DEPT_NO;
//go

//SELECT * FROM V_EMPLEADOS

//create procedure SP_ALL_VEMPLEADOS
//AS
//	SELECT * FROM V_EMPLEADOS
//GO

//EXEC SP_ALL_VEMPLEADOS

//CREATE PROCEDURE SP_INSERT_EMPLEADOS_DINAMICO
//(@apellido nvarchar(50), @oficio nvarchar(50), @dir int, @salario int, @comision int, @nombreDepart nvarchar(50), @id int out)
//AS
//	DECLARE @idDepart int
//	SELECT @idDepart = DEPT_NO FROM DEPT WHERE DNOMBRE=@nombreDepart
//	SELECT @id = MAX(EMP_NO) + 1 FROM EMP
//	insert into EMP values (@id, @apellido, @oficio, @dir, GETDATE(), @salario, @comision, @idDepart);
//GO

//exec SP_INSERT_EMPLEADOS_DINAMICO 'PACO', 'ANALISTA', 7839, 1, 1, 'PRODUCCION'

//DECLARE @idGenerado INT
//EXEC SP_INSERT_EMPLEADOS_DINAMICO 'Sánchez', 'ANALISTA', 7839, 1, 1, 'PRODUCCION', @idGenerado OUTPUT
//print @idGenerado
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosSqlServer: IRepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleadosSqlServer(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<EmpleadosVista>> GetVistaEmpleadosAsync()
        {
            string sql = "SP_ALL_VEMPLEADOS";
            var consulta = this.context.Empleados.FromSqlRaw(sql);
            return await consulta.ToListAsync();

            //var consulta = from datos in this.context.Empleados select datos;
            //return await consulta.ToListAsync();
        }
        public async Task<EmpleadosVista> FindVistaEmpleadosAsync(int idEmpleado)
        {
            var consulta = from datos in this.context.Empleados where datos.IdEmpleado == idEmpleado select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<int> InsertEmpleadosAsync(string apellido, string oficio, int dir, int salario, int comision, string nombreDepart)
        {
            string sql = "SP_INSERT_EMPLEADOS_DINAMICO @apellido, @oficio, @dir, @salario, @comision, @nombreDepart, @id OUT";
            SqlParameter pamApe = new SqlParameter("@apellido", apellido);
            SqlParameter pamOfi = new SqlParameter("@oficio", oficio);
            SqlParameter pamDir = new SqlParameter("@dir", dir);
            SqlParameter pamSala = new SqlParameter("@salario", salario);
            SqlParameter pamComi = new SqlParameter("@comision", comision);
            SqlParameter pamDepart = new SqlParameter("@nombreDepart", nombreDepart);
            SqlParameter pamId = new SqlParameter();
            pamId.ParameterName = "@id";
            pamId.SqlDbType = SqlDbType.Int;
            pamId.Direction = ParameterDirection.Output;
            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamOfi, pamDir, pamSala, pamComi, pamDepart, pamId);
            return (int)pamId.Value;
        }
    }
}
