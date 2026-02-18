using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using Mysqlx.Crud;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Utilities.Zlib;
using System.Collections.Generic;
using System.Data;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region PROCEDURES/VITAS
//create or replace VIEW V_EMPLEADOS
//as
//SELECT EMP.EMP_NO AS IDEMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
//DEPT.DEPT_NO AS IDDEPARTAMENTO, DEPT.DNOMBRE AS DEPARTAMENTO, DEPT.LOC AS LOCALIDAD
//FROM EMP INNER JOIN DEPT ON EMP.DEPT_NO=DEPT.DEPT_NO

//SELECT * from V_EMPLEADOS;

//CREATE OR REPLACE PROCEDURE SP_ALL_VEMPLEADOS
//(p_cursor_empleados OUT SYS_REFCURSOR)
//AS
//BEGIN
//    OPEN p_cursor_empleados FOR SELECT * FROM V_EMPLEADOS;
//END;

//BEGIN
//    SP_ALL_VEMPLEADOS(:cursor_empleados);
//END;

//create procedure SP_INSERT_EMPLEADOS_DINAMICO
//(p_apellido EMP.APELLIDO%TYPE, p_oficio EMP.OFICIO%TYPE, p_dir EMP.DIR%TYPE, p_salario EMP.SALARIO%TYPE, p_comision EMP.COMISION%TYPE, p_nombreDepart DEPT.DNOMBRE%TYPE, p_id OUT EMP.EMP_NO%TYPE)
//AS
//p_idDepartamento DEPT.DEPT_NO%TYPE;
//BEGIN
//    SELECT MAX(EMP_NO)+1 INTO p_id FROM EMP;
//SELECT DEPT_NO INTO p_idDepartamento FROM DEPT WHERE DNOMBRE=p_nombreDepart;
//INSERT INTO EMP VALUES(p_id, p_apellido, p_oficio, p_dir, SYSDATE, p_salario, p_comision, p_idDepartamento);
//COMMIT;
//END;

//SET SERVEROUTPUT ON;
//DECLARE
//    v_id_nuevo EMP.EMP_NO%TYPE;
//BEGIN
//    SP_INSERT_EMPLEADOS_DINAMICO('PRUEBA3', 'PRUEBA3', 7839, 10, 5, 'CONTABILIDAD', v_id_nuevo);
//    DBMS_OUTPUT.PUT_LINE('El ID generado para el nuevo empleado es: ' || v_id_nuevo);
//END;
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosOracle: IRepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleadosOracle(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<EmpleadosVista>> GetVistaEmpleadosAsync()
        {
            string sql = "begin ";
            sql += " SP_ALL_VEMPLEADOS(:p_cursor_empleados); ";
            sql += " end;";

            OracleParameter pamCursor = new OracleParameter();
            pamCursor.ParameterName = "p_cursor_empleados";
            pamCursor.Value = null;
            pamCursor.Direction = ParameterDirection.Output;
            //Indicamos el tipo de oracle
            pamCursor.OracleDbType = OracleDbType.RefCursor;
            var consulta = this.context.Empleados.FromSqlRaw(sql, pamCursor);
            return await consulta.ToListAsync();
        }
        public async Task<EmpleadosVista> FindVistaEmpleadosAsync(int idEmpleado)
        {
            var consulta = from datos in this.context.Empleados where datos.IdEmpleado == idEmpleado select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<int> InsertEmpleadosAsync(string apellido, string oficio, int dir, int salario, int comision, string nombreDepart)
        {
            string sql = "begin ";
            sql += " SP_INSERT_EMPLEADOS_DINAMICO(:p_apellido, :p_oficio, :p_dir, :p_salario, :p_comision, :p_nombreDepart, :p_id); ";
            sql += " end;";
            OracleParameter pamApe = new OracleParameter(":p_apellido", apellido);
            OracleParameter pamOfi = new OracleParameter(":p_oficio", oficio);
            OracleParameter pamDir = new OracleParameter(":p_dir", dir);
            OracleParameter pamSala = new OracleParameter(":p_salario", salario);
            OracleParameter pamComi = new OracleParameter(":p_comision", comision);
            OracleParameter pamDepart = new OracleParameter(":p_nombreDepart", nombreDepart);

            OracleParameter pamId = new OracleParameter();
            pamId.ParameterName = ":p_id";
            pamId.OracleDbType = OracleDbType.Int32;
            pamId.Direction = ParameterDirection.Output;

            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamOfi, pamDir, pamSala, pamComi, pamDepart, pamId);
            return Convert.ToInt32(pamId.Value.ToString());
        }
    }
}
