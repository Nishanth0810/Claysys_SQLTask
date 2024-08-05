using Claysys_SQLTask.Models;
using Claysys_SQLTask.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Claysys_SQLTask.Controllers
{
    public class LoginController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]  
        public IActionResult Login(LoginModel loginModel)
        {
            int EmpId = 0;
            string UserName = null;
            string Role = null;
            LoginRepository loginRepo = new LoginRepository(_configuration);
            bool IsValid = loginRepo.IsValidUser(loginModel, EmpId, UserName, Role,_httpContextAccessor); 
            if (IsValid)
            {
                var session = _httpContextAccessor.HttpContext.Session;
                EmpId = (int)session.GetInt32("EmpId");
                UserName = session.GetString("UserName");
                Role = session.GetString("Role");
                return View();
            }
            
            return View(loginModel);
            
        }
        
    }
}
