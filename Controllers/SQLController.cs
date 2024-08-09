using Microsoft.AspNetCore.Mvc;
using Claysys_SQLTask.Models;
using Claysys_SQLTask.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;

namespace Claysys_SQLTask.Controllers
{
    public class SQLController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserRepository _userRepository;
        public SQLController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, UserRepository userRepository)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }
        [HttpGet]
        [Route("Home")]
        public IActionResult Home()
        {
            int CreatedBy = _httpContextAccessor.HttpContext.Session.GetInt32("EmpId") ?? 2106;
            var data = _userRepository.GetHomeModelByUser(CreatedBy);
            return View(data);
        }
        [HttpGet]
        public IActionResult Database()
        {
            UserRepository userRepo = new UserRepository(_configuration);
            ViewBag.Clients = userRepo.GetClients();
            return View();
        }

        [HttpPost]
        public IActionResult Database(Database database)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            bool result = userRepo.InsertDatabase(database);
            if (result)
            {
                return RedirectToAction("Home");
            }
            return View();
        }

        [HttpGet]
        [Route("SQL/Table")]
        public IActionResult AddTable()
        {
            //if (_httpContextAccessor.HttpContext.Session.GetString("userName") != null)
            //{
            //    UserRepository userRepo = new UserRepository(_configuration);
            //    ViewBag.Clients = userRepo.GetClients();
            //    return View();
            //}
            //return RedirectToAction("Login", "Login");
            UserRepository userRepo = new UserRepository(_configuration);
            ViewBag.Clients = userRepo.GetClients();
            return View();
        }

        [HttpPost]
        public IActionResult AddTable(Tables table)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("EmpId");
            bool result = userRepo.InsertTable(table, CreatedBy);
            if (result)
            {
                return RedirectToAction("Home");
            }
            return View();
        }


        [HttpGet]
        [Route("SQL/Procedure")]
        public IActionResult AddProcedure()
        {
            UserRepository userRepo = new UserRepository(_configuration);
            ViewBag.Clients = userRepo.GetClients();
            return View();
        }

        [HttpPost]
        public IActionResult AddProcedure(Procedures procedure)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("EmpId");
            bool result = userRepo.InsertProcedure(procedure, CreatedBy);
            if (result)
            {
                return RedirectToAction("Home");
            }
            return View();
        }

        [HttpGet]
        [Route("SQL/Index")]
        public IActionResult AddIndex()
        {
            UserRepository userRepo = new UserRepository(_configuration);
            ViewBag.Clients = userRepo.GetClients();
            return View();
        }

        [HttpPost]
        public IActionResult AddIndex(Indexes index)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("EmpId");
            bool result = userRepo.InsertIndex(index, CreatedBy);
            if (result)
            {
                return RedirectToAction("Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Client()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Client(Client client)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            bool result = userRepo.InsertClient(client);
            if (result)
            {
                return RedirectToAction("Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Project()
        {
            UserRepository userRepo = new UserRepository(_configuration);
            ViewBag.Clients = userRepo.GetClients();
            return View();
        }

        [HttpPost]
        public IActionResult Project(Project project)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            bool result = userRepo.InsertProject(project);
            if (result)
            {
                return RedirectToAction("Home");
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetProjects(int clientId)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var projects = userRepo.GetProjects(clientId);
            return Json(projects);
        }

        [HttpPost]
        public JsonResult GetDataBases(int projectId)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var databases = userRepo.GetDataBases(projectId);
            return Json(databases);
        }

        [HttpPost]
        public JsonResult GetTables(int clientId, int projectId, int databaseId)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var tables = userRepo.GetTables(clientId, projectId, databaseId);
            return Json(tables);
        }

        [HttpGet]
        public IActionResult ProcedureList()
        {
            List<Procedures> procedures = new List<Procedures>();
            UserRepository userRepo = new UserRepository(_configuration);
            procedures = userRepo.GetProcedures();
            return View(procedures);
        }

        [HttpGet]
        public IActionResult ProcedureReview(int SPID)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            SpReview spReview = new SpReview();
            spReview = userRepo.GetProcedureById(SPID);
            ViewBag.ClientName = spReview.ClientName;
            ViewBag.ProjectName = spReview.ProjectName;
            ViewBag.SPID = spReview.SPID;
            ViewBag.SPName = spReview.SPName;
            ViewBag.DatabaseName = spReview.DatabaseName;
            return View();
        }
        [HttpPost]
        public IActionResult ProcedureReview(SpReview spReview)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("EmpId");
            bool result = userRepo.InsertSPReview(spReview, CreatedBy);
            if (result)
            {
                return RedirectToAction("Home");
            }
            return RedirectToAction("Home");
        }

        [HttpGet]
        public IActionResult ReviewList(int SPID)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            List<SpReview> spReviews = new List<SpReview>();
            spReviews = userRepo.GetReviewsById(SPID);
            SpReview spReview = new SpReview();
            spReview = userRepo.GetProcedureById(SPID);
            ViewBag.SPName = spReview.SPName;
            return View(spReviews);
        }

        public async Task<IActionResult> GetFilteredData(string filters, int page = 1, int pageSize = 5)
        {

            var dataTable = _userRepository.GetSpDetails(filters, page, pageSize);
            var totalRecords = _userRepository.GetSpCount(filters, page, pageSize);
            var result = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }
                result.Add(dict);
            }

            return Json(new { data = result, totalRecords });
        }

        public async Task<IActionResult> GetindexFilteredData(string filters, int page = 1, int pageSize = 5)
        {

            var dataTable = _userRepository.GetIndexDetails(filters, page, pageSize);
            var totalRecords = _userRepository.GetIndexCount(filters, page, pageSize);
            var result = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }
                result.Add(dict);
            }

            return Json(new { data = result, totalRecords });
        }

        public async Task<IActionResult> GetClientData(string filters, int page = 1, int pageSize = 5)
        {

            var dataTable = _userRepository.GetClientDetails(filters, page, pageSize);
            var totalRecords = _userRepository.GetClientCount(filters, page, pageSize);
            var result = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }
                result.Add(dict);
            }

            return Json(new { data = result, totalRecords });
        }

        public async Task<IActionResult> GetProjectData(string filters, int page = 1, int pageSize = 5)
        {

            var dataTable = _userRepository.GetProjectDetails(filters, page, pageSize);
            var totalRecords = _userRepository.GetProjectCount(filters, page, pageSize);
            var result = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }
                result.Add(dict);
            }

            return Json(new { data = result, totalRecords });
        }

        public async Task<IActionResult> GetDbData(string filters, int page = 1, int pageSize = 5)
        {

            var dataTable = _userRepository.GetDbDetails(filters, page, pageSize);
            var totalRecords = _userRepository.GetDbCount(filters, page, pageSize);
            var result = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }
                result.Add(dict);
            }

            return Json(new { data = result, totalRecords });
        }

        public async Task<IActionResult> GetTableData(string filters, int page = 1, int pageSize = 5)
        {

            var dataTable = _userRepository.GetTableDetails(filters, page, pageSize);
            var totalRecords = _userRepository.GetTableCount(filters, page, pageSize);
            var result = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column];
                }
                result.Add(dict);
            }

            return Json(new { data = result, totalRecords });
        }


    }

}

   