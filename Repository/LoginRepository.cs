using Claysys_SQLTask.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Data;
using System.Security.Claims;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Claysys_SQLTask.Repository
{
    public class LoginRepository
    {
        private readonly string _connectionString;
        public LoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool IsValidUser(LoginModel loginModel,int EmpId, string UserName, string Role, IHttpContextAccessor httpContextAccessor)
        {
            bool isValidUser = false;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Sp_IsValidUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("UserName", loginModel.UserName);
                    cmd.Parameters.AddWithValue("Password", loginModel.Password);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmpId = Convert.ToInt32(reader["EmpId"]);
                            UserName = reader["UserName"].ToString();
                            Role = reader["Role"].ToString();
                        }    
                        con.Close();
                    }
                    if (EmpId!=0)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, UserName),
                            new Claim(ClaimTypes.Role, Role)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true
                        };

                         httpContextAccessor.HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                        var session = httpContextAccessor.HttpContext.Session;
                        session.SetInt32("EmpId", EmpId);
                        session.SetString("UserName", UserName);
                        session.SetString("Role", Role);
                        isValidUser = true;
                    }

                }
            }
            return isValidUser;
        }
    }
}
