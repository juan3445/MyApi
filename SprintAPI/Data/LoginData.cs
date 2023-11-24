using Microsoft.IdentityModel.Tokens;
using SprintAPI.Context;
using SprintAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;


namespace SprintAPI.Data
{
    public class LoginData
    {
        SprintsContext cnx = new SprintsContext();
       

        public async Task<bool>ValidarUsuario(UsuarioModel user)
        {
            using (var sql = new SqlConnection(cnx.cadenaSQL()))
            {
                await sql.OpenAsync();
                string query = "SELECT id FROM Usuario WHERE cuenta = @usuario AND contrasena = @clave";
                using (var cmd = new SqlCommand(query, sql))
                {
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 30).Value = user.Cuenta;
                    cmd.Parameters.Add("@clave", SqlDbType.VarChar, 30).Value = user.Clave;
                    await cmd.PrepareAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync();
                    }

                }
            }
        }
        public string GenerarToken(string usuario) {
            var keyString = cnx.secretKey();
            var TokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(keyString);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = TokenHandler.CreateToken(tokenDescriptor);
            return TokenHandler.WriteToken(token);  
        }
    }
}
