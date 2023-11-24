using SprintAPI.Context;
using SprintAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace SprintAPI.Data
{
    public class EstudianteData
    {
        SprintsContext cnx = new SprintsContext();
        private bool esCodigoMunicipioValido;

        public async Task<List<EstudianteModel>> GetEstudiante()
        {
            var lista = new List<EstudianteModel>();
            using (var sql = new SqlConnection(cnx.cadenaSQL()))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("SELECT * FROM Estudiantes", sql))
                {
                    await cmd.PrepareAsync();
                    using (var item = await cmd.ExecuteReaderAsync())
                    {
                        while (await item.ReadAsync())
                        {
                            var estudiante = new EstudianteModel();
                            estudiante.Identificacion = (string)item["Identificacion"];
                            estudiante.Nombre1 = (string)item["Nombre1"];
                            estudiante.Nombre2 = item["Nombre2"] != DBNull.Value ? (string)item["Nombre2"] : null;
                            estudiante.Apellido1 = (string)item["Apellido1"];
                            estudiante.Apellido2 = item["Apellido2"] != DBNull.Value ? (string)item["Apellido2"] : null;
                            estudiante.EstadoCivil = (string)item["EstadoCivil"];
                            estudiante.FechaNacimiento = (DateTime)item["FechaNacimiento"];
                            estudiante.Genero = (string)item["Genero"];
                            estudiante.Ocupacion = (string)item["Ocupacion"];
                            estudiante.FechaInicio = (DateTime)item["FechaInicio"];
                            estudiante.ViveCon = (string)item["ViveCon"];
                            estudiante.Municipio = (string)item["Municipio"];
                            estudiante.NroHijos = (int)item["NroHijos"];
                            estudiante.NivelFormacion = (string)item["NivelFormacion"];
                            estudiante.Estrato = item["Estrato"] != DBNull.Value ? (string)item["Estrato"] : null;
                            estudiante.IngresosMonetarios = (int)item["IngresosMonetarios"];
                            estudiante.EmpresaDondeTrabaja = item["EmpresaDondeTrabaja"] != DBNull.Value ? (string)item["EmpresaDondeTrabaja"] : null;
                            estudiante.Retencion = (Boolean)item["Retencion"];
                            lista.Add(estudiante);
                        }
                    }
                }
            }
            return lista;
        }

        public async Task<List<EstudianteModel>> GetEstudianteById(string id)
        {
            var lista = new List<EstudianteModel>();
            using (var sql = new SqlConnection(cnx.cadenaSQL()))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand("SELECT Identificacion, Nombre1, Nombre2, Apellido1, Apellido2, EstadoCivil, FechaNacimiento, Genero, Ocupacion, FechaInicio, ViveCon, Municipio, NroHijos, NivelFormacion, Estrato, IngresosMonetarios, EmpresaDondeTrabaja, Retencion FROM Estudiantes WHERE Identificacion=@Id", sql))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.VarChar, 12).Value = id;
                    await cmd.PrepareAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var estudiante = new EstudianteModel
                            {
                                Identificacion = (string)reader["Identificacion"],
                                Nombre1 = (string)reader["Nombre1"],
                                Nombre2 = reader["Nombre2"] != DBNull.Value ? (string)reader["Nombre2"] : null,
                                Apellido1 = (string)reader["Apellido1"],
                                Apellido2 = reader["Apellido2"] != DBNull.Value ? (string)reader["Apellido2"] : null,
                                EstadoCivil = (string)reader["EstadoCivil"],
                                FechaNacimiento = (DateTime)reader["FechaNacimiento"],
                                Genero = (string)reader["EstadoCivil"],
                                Ocupacion = (string)reader["EstadoCivil"],
                                FechaInicio = (DateTime)reader["FechaNacimiento"],
                                ViveCon = (string)reader["ViveCon"],
                                Municipio = (string)reader["Municipio"],
                                NroHijos = (int)reader["NroHijos"],
                                NivelFormacion = (string)reader["NivelFormacion"],
                                Estrato = reader["Estrato"] != DBNull.Value ? (string)reader["Estrato"] : null,
                                IngresosMonetarios = (int)reader["IngresosMonetarios"],
                                EmpresaDondeTrabaja = reader["EmpresaDondeTrabaja"] != DBNull.Value ? (string)reader["EmpresaDondeTrabaja"] : null,
                                Retencion = (Boolean)reader["Retencion"]
                            };
                            lista.Add(estudiante);
                        }
                    }
                }
            }
            return lista;
        }

        public async Task<List<EstudianteModel>> GetEstudiantes(List<string> identificaciones)
        {
            var lista = new List<EstudianteModel>();

            using (var sql = new SqlConnection(cnx.cadenaSQL()))
            {
                await sql.OpenAsync();

                var sqlCommandText = "SELECT Identificacion, Nombre1, Nombre2, Apellido1, Apellido2, EstadoCivil, FechaNacimiento, Genero, Ocupacion, FechaInicio, ViveCon, Municipio, NroHijos, NivelFormacion, Estrato, IngresosMonetarios, EmpresaDondeTrabaja, Retencion FROM Estudiantes WHERE Identificacion IN ({0})";
                var parameterNames = string.Join(", ", identificaciones.Select((id, index) => $"@id{index}"));
                var commandText = string.Format(sqlCommandText, parameterNames);

                using (var cmd = new SqlCommand(commandText, sql))
                {
                    for (int i = 0; i < identificaciones.Count; i++)
                    {
                        cmd.Parameters.Add($"@id{i}", SqlDbType.VarChar, 12).Value = identificaciones[i];

                        var existe = (await GetEstudianteById(identificaciones[i])).FirstOrDefault();
                        if (!Regex.IsMatch(identificaciones[i], "^[0-9]+$"))
                        {
                            Log($"El documento {identificaciones[i]} debe contener únicamente números.");
                            continue;
                        }
                        else if (existe == null)
                        {
                            Log($"El estudiante con identificación {identificaciones[i]} no se encuentra registrado.");
                            continue;
                        }
                        
                    }
                    //foreach (var identificacion in identificaciones)
                    //{
                    //    var existe = (await GetEstudianteById(identificacion)).FirstOrDefault();
                    //    if (!Regex.IsMatch(identificacion, "^[0-9]+$"))
                    //    {
                    //        Log($"El documento {identificacion} debe contener únicamente números.");
                    //        continue;
                    //    }
                    //    else if (existe == null)
                    //    {
                    //        Log($"El estudiante con identificación {identificacion} no se encuentra registrado.");
                    //        continue;
                    //    }

                    try
                    {
                        await cmd.PrepareAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var estudiante = new EstudianteModel
                                {
                                    Identificacion = (string)reader["Identificacion"],
                                    Nombre1 = (string)reader["Nombre1"],
                                    Nombre2 = reader["Nombre2"] != DBNull.Value ? (string)reader["Nombre2"] : null,
                                    Apellido1 = (string)reader["Apellido1"],
                                    Apellido2 = reader["Apellido2"] != DBNull.Value ? (string)reader["Apellido2"] : null,
                                    EstadoCivil = (string)reader["EstadoCivil"],
                                    FechaNacimiento = (DateTime)reader["FechaNacimiento"],
                                    Genero = (string)reader["EstadoCivil"],
                                    Ocupacion = (string)reader["EstadoCivil"],
                                    FechaInicio = (DateTime)reader["FechaNacimiento"],
                                    ViveCon = (string)reader["ViveCon"],
                                    Municipio = (string)reader["Municipio"],
                                    NroHijos = (int)reader["NroHijos"],
                                    NivelFormacion = (string)reader["NivelFormacion"],
                                    Estrato = reader["Estrato"] != DBNull.Value ? (string)reader["Estrato"] : null,
                                    IngresosMonetarios = (int)reader["IngresosMonetarios"],
                                    EmpresaDondeTrabaja = reader["EmpresaDondeTrabaja"] != DBNull.Value ? (string)reader["EmpresaDondeTrabaja"] : null,
                                    Retencion = (Boolean)reader["Retencion"]
                                };
                                lista.Add(estudiante);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"Error al consultar estudiante con identificación {identificaciones}: {ex.Message}");
                        //continue;
                    }
                //}
            }
        }

          return lista;
    }

        public async Task PostEstudiante(EstudianteModel estudiante)
        {
            using (var sql = new SqlConnection(cnx.cadenaSQL()))
            {
                await sql.OpenAsync();
                string query = "INSERT INTO Estudiantes (Identificacion, Nombre1, Nombre2, Apellido1, Apellido2, EstadoCivil, FechaNacimiento, Genero, Ocupacion, FechaInicio, ViveCon, Municipio, NroHijos, NivelFormacion, Estrato, IngresosMonetarios, EmpresaDondeTrabaja, Retencion) " +
                                "VALUES (@Id, @Nombre1, @Nombre2, @Apellido1, @Apellido2, @EstadoCivil, @FechaNacimiento, @Genero, @Ocupacion, @FechaInicio, @ViveCon, @Municipio, @NroHijos, @NivelFormacion, @Estrato, @IngresosMonetarios, @EmpresaDondeTrabaja, @Retencion)";
                using (var cmd = new SqlCommand(query, sql))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.VarChar, 12).Value = estudiante.Identificacion;
                    cmd.Parameters.Add("@Nombre1", SqlDbType.VarChar, 50).Value = estudiante.Nombre1;
                    cmd.Parameters.Add("@Nombre2", SqlDbType.VarChar, 50).Value = estudiante.Nombre2 ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Apellido1", SqlDbType.VarChar, 50).Value = estudiante.Apellido1;
                    cmd.Parameters.Add("@Apellido2", SqlDbType.VarChar, 50).Value = estudiante.Apellido2 ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@EstadoCivil", SqlDbType.VarChar, 5).Value = estudiante.EstadoCivil;
                    cmd.Parameters.Add("@FechaNacimiento", SqlDbType.DateTime).Value = estudiante.FechaNacimiento;
                    cmd.Parameters.Add("@Genero", SqlDbType.VarChar, 5).Value = estudiante.Genero;
                    cmd.Parameters.Add("@Ocupacion", SqlDbType.VarChar, 5).Value = estudiante.Ocupacion;
                    cmd.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = estudiante.FechaInicio;
                    cmd.Parameters.Add("@ViveCon", SqlDbType.VarChar, 5).Value = estudiante.ViveCon;
                    cmd.Parameters.Add("@Municipio", SqlDbType.VarChar, 5).Value = estudiante.Municipio;
                    cmd.Parameters.Add("@NroHijos", SqlDbType.Int).Value = estudiante.NroHijos;
                    cmd.Parameters.Add("@NivelFormacion", SqlDbType.VarChar, 5).Value = estudiante.NivelFormacion;
                    cmd.Parameters.Add("@Estrato", SqlDbType.VarChar, 5).Value = estudiante.Estrato ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@IngresosMonetarios", SqlDbType.Int).Value = estudiante.IngresosMonetarios;
                    cmd.Parameters.Add("@EmpresaDondeTrabaja", SqlDbType.VarChar, 50).Value = estudiante.EmpresaDondeTrabaja ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Retencion", SqlDbType.Bit).Value = estudiante.Retencion;
                    await cmd.PrepareAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task PostEstudiantes(List<EstudianteModel> estudiantes)
        {
            using (var sql = new SqlConnection(cnx.cadenaSQL()))
            {
                await sql.OpenAsync();
                string query = "INSERT INTO Estudiantes (Identificacion, Nombre1, Nombre2, Apellido1, Apellido2, EstadoCivil, FechaNacimiento, Genero, Ocupacion, FechaInicio, ViveCon, Municipio, NroHijos, NivelFormacion, Estrato, IngresosMonetarios, EmpresaDondeTrabaja, Retencion) " +
                                "VALUES (@Id, @Nombre1, @Nombre2, @Apellido1, @Apellido2, @EstadoCivil, @FechaNacimiento, @Genero, @Ocupacion, @FechaInicio, @ViveCon, @Municipio, @NroHijos, @NivelFormacion, @Estrato, @IngresosMonetarios, @EmpresaDondeTrabaja, @Retencion)";
                foreach (var estudiante in estudiantes)
                {
                    var existe =  (await GetEstudianteById(estudiante.Identificacion)).FirstOrDefault();
                    var esMunicipioValido = await ValidarCodigo(estudiante.Municipio, 1);
                    var esEstadoCivilValido = await ValidarCodigo(estudiante.EstadoCivil, 2);
                    var esGeneroValido = await ValidarCodigo(estudiante.Genero, 2);
                    var esOcupacionValido = await ValidarCodigo(estudiante.Ocupacion, 2);
                    var esViveConValido = await ValidarCodigo(estudiante.ViveCon, 2);
                    var esNivelFormacionValido = await ValidarCodigo(estudiante.NivelFormacion, 2);
                    var esEstratoValido = await ValidarCodigo(estudiante.Estrato, 2);

                    if (!Regex.IsMatch(estudiante.Identificacion, "^[0-9]+$"))
                    {
                        Log($"El documento {estudiante.Identificacion} debe contener únicamente números.");
                        continue;
                    }
                    else if (existe!=null)
                    {
                        Log($"El estudiante con identificación {estudiante.Identificacion} ya existe y se encuentra registrado.");
                        continue;
                    }
                    else if (!esMunicipioValido || !esEstadoCivilValido || !esGeneroValido || !esOcupacionValido || !esViveConValido || !esNivelFormacionValido || !esEstratoValido)
                    {
                        Log($"El código Municipio, Estado Civil, Genero, Ocupacion, ViveCon, Nivel Formacion o Estrato ingresado no es válido.");
                        continue;
                    }

                    try
                    {
                        using (var cmd = new SqlCommand(query, sql))
                        {
                            cmd.Parameters.Add("@Id", SqlDbType.VarChar, 12).Value = estudiante.Identificacion;
                            cmd.Parameters.Add("@Nombre1", SqlDbType.VarChar, 30).Value = estudiante.Nombre1;
                            cmd.Parameters.Add("@Nombre2", SqlDbType.VarChar, 30).Value = estudiante.Nombre2 ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@Apellido1", SqlDbType.VarChar, 30).Value = estudiante.Apellido1;
                            cmd.Parameters.Add("@Apellido2", SqlDbType.VarChar, 30).Value = estudiante.Apellido2 ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@EstadoCivil", SqlDbType.VarChar, 5).Value = estudiante.EstadoCivil;
                            cmd.Parameters.Add("@FechaNacimiento", SqlDbType.DateTime).Value = estudiante.FechaNacimiento; 
                            cmd.Parameters.Add("@Genero", SqlDbType.VarChar, 5).Value = estudiante.Genero;
                            cmd.Parameters.Add("@Ocupacion", SqlDbType.VarChar, 5).Value = estudiante.Ocupacion;
                            cmd.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = estudiante.FechaInicio;
                            cmd.Parameters.Add("@ViveCon", SqlDbType.VarChar, 5).Value = estudiante.ViveCon;
                            cmd.Parameters.Add("@Municipio", SqlDbType.VarChar, 5).Value = estudiante.Municipio;
                            cmd.Parameters.Add("@NroHijos", SqlDbType.Int).Value = estudiante.NroHijos;
                            cmd.Parameters.Add("@NivelFormacion", SqlDbType.VarChar, 5).Value = estudiante.NivelFormacion;
                            cmd.Parameters.Add("@Estrato", SqlDbType.VarChar, 5).Value = estudiante.Estrato ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@IngresosMonetarios", SqlDbType.Int).Value = estudiante.IngresosMonetarios;
                            cmd.Parameters.Add("@EmpresaDondeTrabaja", SqlDbType.VarChar, 50).Value = estudiante.EmpresaDondeTrabaja ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@Retencion", SqlDbType.Bit).Value = estudiante.Retencion;
                            await cmd.PrepareAsync();
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"Error al registrar estudiante con identificación {estudiante.Identificacion}: {ex.Message}");
                        continue;
                    }
                    
                }
            }
        }

        public void Log(string message)
        {
            string formattedDate = DateTime.Now.ToString("dd-MM-yyyy HHmmss");
            string logFilePath = $@"D:\logsAPI\log_errors_{formattedDate}.txt";

            File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");
        }



        public async Task<bool> ValidarCodigo(string codigo, int tipo)
        {
            string query = "";
            if (tipo == 1)
            {
                query = "SELECT Codigo, Nombre FROM Municipio WHERE Codigo = @cod";
            }
            else if (tipo == 2)
            {
                query = "SELECT Codigo, Nombre FROM Concepto WHERE Codigo = @cod";
            }
            using (var sql = new SqlConnection(cnx.cadenaSQL()))
            {
                await sql.OpenAsync();
                using (var cmd = new SqlCommand(query, sql))
                {
                    cmd.Parameters.Add("@cod", SqlDbType.VarChar, 5).Value = codigo;
                    await cmd.PrepareAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync();
                    }
                }
            }

        }
    }
}
