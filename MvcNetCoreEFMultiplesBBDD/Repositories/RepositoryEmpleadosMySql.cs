using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Utilities.Zlib;
using System.Collections.Generic;
using System.Data;

#region PROCEDURES/VITAS
//create VIEW V_EMPLEADOS
//as
//SELECT EMP.EMP_NO AS IDEMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
//DEPT.DEPT_NO AS IDDEPARTAMENTO, DEPT.DNOMBRE AS DEPARTAMENTO, DEPT.LOC AS LOCALIDAD
//FROM EMP INNER JOIN DEPT ON EMP.DEPT_NO=DEPT.DEPT_NO

//SELECT * from V_EMPLEADOS;

//DELIMITER $$
//CREATE PROCEDURE SP_ALL_VEMPLEADOS()
//BEGIN
//    SELECT * FROM V_EMPLEADOS;
//END $$
//DELIMITER ;

//CALL SP_ALL_VEMPLEADOS();

//DELIMITER $$
//CREATE PROCEDURE SP_INSERT_EMPLEADOS_DINAMICO
//(IN p_apellido VARCHAR(50), IN p_oficio VARCHAR(50), IN p_dir INT, IN p_salario INT, IN p_comision INT, IN p_nombreDepart VARCHAR(50), OUT p_id INT)
//BEGIN
//    DECLARE v_idDepart INT;
//SELECT DEPT_NO INTO v_idDepart FROM DEPT WHERE DNOMBRE = p_nombreDepart;
//SELECT MAX(EMP_NO) +1 INTO p_id FROM EMP;
//INSERT INTO EMP VALUES (p_id, p_apellido, p_oficio, p_dir, NOW(), p_salario, p_comision, v_idDepart);
//END $$
//DELIMITER ;

//CALL SP_INSERT_EMPLEADOS_DINAMICO('ALONSO MYSQL', 'PROGRAMADOR', 7902, 2800, 100, 'VENTAS MYSQL', @id_generado);
//SELECT @id_generado AS 'ID Insertado';
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosMySql: IRepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleadosMySql(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<EmpleadosVista>> GetVistaEmpleadosAsync()
        {
            string sql = "CALL SP_ALL_VEMPLEADOS()";
            var consulta = this.context.Empleados.FromSqlRaw(sql);
            return await consulta.ToListAsync();
        }
        public async Task<EmpleadosVista> FindVistaEmpleadosAsync(int idEmpleado)
        {
            var consulta = from datos in this.context.Empleados where datos.IdEmpleado == idEmpleado select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<int> InsertEmpleadosAsync(string apellido, string oficio, int dir, int salario, int comision, string nombreDepart)
        {
            string sql = "CALL SP_INSERT_EMPLEADOS_DINAMICO(@p_apellido, @p_oficio, @p_dir, @p_salario, @p_comision, @p_nombreDepart, @p_id)";
            MySqlParameter pamApe = new MySqlParameter("@p_apellido", apellido);
            MySqlParameter pamOfi = new MySqlParameter("@p_oficio", oficio);
            MySqlParameter pamDir = new MySqlParameter("@p_dir", dir);
            MySqlParameter pamSala = new MySqlParameter("@p_salario", salario);
            MySqlParameter pamComi = new MySqlParameter("@p_comision", comision);
            MySqlParameter pamDepart = new MySqlParameter("@p_nombreDepart", nombreDepart);

            MySqlParameter pamId = new MySqlParameter
            {
                ParameterName = "@p_id",
                MySqlDbType = MySqlDbType.Int32,
                Direction = ParameterDirection.Output
            };

            await this.context.Database.ExecuteSqlRawAsync(sql, pamApe, pamOfi, pamDir, pamSala, pamComi, pamDepart, pamId);
            return (int)pamId.Value;
        }
    }
}