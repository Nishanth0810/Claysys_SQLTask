using Microsoft.AspNetCore.Mvc;
using Claysys_SQLTask.Models;
using Claysys_SQLTask.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis;

namespace Claysys_SQLTask.Controllers
{
    public class SQLController:Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SQLController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        [Route("Home")]
        public IActionResult Home()
        {
            return View();
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
        [Route("SQL/Table")]
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
        [Route("SQL/Procedure")]
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
        [Route("SQL/Index")]
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
        public IActionResult Project(Claysys_SQLTask.Models.Project project)
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

        [HttpPost]
        public JsonResult GetIndexes(int clientId, int projectId, int databaseId)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var tables = userRepo.GetIndexes(clientId, projectId, databaseId);
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
            spReview=userRepo.GetProcedureById(SPID);
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
            ProcedureRelation procedureTableRelation = new ProcedureRelation();
            procedureTableRelation = userRepo.GetProcedureTableRelationById(SPID);
            ProcedureRelation procedureIndexRelation = new ProcedureRelation();
            procedureIndexRelation = userRepo.GetProcedureIndexRelationById(SPID);
            ViewBag.SPName = procedureTableRelation.SPName;
            ViewBag.TableName = procedureTableRelation.TableName;
            ViewBag.IndexName = procedureIndexRelation.IndexName;
            return View(spReviews);
        }

        [HttpGet]
        public IActionResult ProcedureTableRelation(int SPID)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            ProcedureRelation procedureRelation = new ProcedureRelation();
            procedureRelation = userRepo.GetProcedureRelationById(SPID);
            ViewBag.Tables = userRepo.GetTables(procedureRelation.ClientID, procedureRelation.ProjectID, procedureRelation.DataBaseID);
            ViewBag.ClientName = procedureRelation.ClientName;
            ViewBag.ProjectName = procedureRelation.ProjectName;
            ViewBag.SPID = procedureRelation.SPID;
            ViewBag.SPName = procedureRelation.SPName;
            ViewBag.DatabaseName = procedureRelation.DataBaseName;
            return View();
        }

        [HttpPost]
        public IActionResult ProcedureTableRelation(ProcedureRelation procedureRelation)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("EmpId");
            bool result = userRepo.InsertProcedureTableRelation(procedureRelation, CreatedBy);
            if (result)
            {
                return RedirectToAction("Home");
            }
            return RedirectToAction("Home");
        }

        [HttpGet]
        public IActionResult ProcedureIndexRelation(int SPID)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            ProcedureRelation procedureRelation = new ProcedureRelation();
            procedureRelation = userRepo.GetProcedureRelationById(SPID);
            ViewBag.Indexes = userRepo.GetIndexes(procedureRelation.ClientID, procedureRelation.ProjectID, procedureRelation.DataBaseID);
            ViewBag.ClientName = procedureRelation.ClientName;
            ViewBag.ProjectName = procedureRelation.ProjectName;
            ViewBag.SPID = procedureRelation.SPID;
            ViewBag.SPName = procedureRelation.SPName;
            ViewBag.DatabaseName = procedureRelation.DataBaseName;
            return View();
        }

        [HttpPost]
        public IActionResult ProcedureIndexRelation(ProcedureRelation procedureRelation)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("EmpId");
            bool result = userRepo.InsertProcedureIndexRelation(procedureRelation, CreatedBy);
            if (result)
            {
                return RedirectToAction("Home");
            }
            return RedirectToAction("Home");
        }

        [HttpPost]
        public IActionResult ReviewMove(int id,string fieldName, bool value)
        {
            UserRepository userRepo = new UserRepository(_configuration);
            var CreatedBy = (int)_httpContextAccessor.HttpContext.Session.GetInt32("EmpId");
            bool result = userRepo.UpdateReviewChanges(id, fieldName, value,CreatedBy);
            if (result)
            {
                return Ok();
            }
            return NotFound();

        }


    }
}
