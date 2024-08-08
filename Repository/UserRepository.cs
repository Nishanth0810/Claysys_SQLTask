using Claysys_SQLTask.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Buffers;
using System.Configuration;
using System.Data;
using System.Security.Policy;
using static Claysys_SQLTask.Models.HomeContentModel;
using static NuGet.Packaging.PackagingConstants;

namespace Claysys_SQLTask.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Client> GetClients()
        {
            var clients = new List<Client>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT ClientID, ClientName FROM Clients WHERE IsActive = 1", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clients.Add(new Client
                        {
                            ClientID = (int)reader["ClientID"],
                            ClientName = reader["ClientName"].ToString()
                        });
                    }
                }
            }
            return clients;
        }

        public List<Claysys_SQLTask.Models.Project> GetProjects(int clientId)
        {
            var projects = new List<Claysys_SQLTask.Models.Project>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT ProjectID, ProjectName FROM Projects WHERE ClientID = @ClientID AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@ClientID", clientId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        projects.Add(new Claysys_SQLTask.Models.Project
                        {
                            ProjectID = (int)reader["ProjectID"],
                            ProjectName = reader["ProjectName"].ToString()
                        });
                    }
                }
            }
            return projects;
        }

        public List<Database> GetDataBases(int projectId)
        {
            var databases = new List<Database>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT DataBaseID, DataBaseName FROM DataBases WHERE ProjectID = @ProjectID AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@ProjectID", projectId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databases.Add(new Database
                        {
                            DataBaseID = (int)reader["DataBaseID"],
                            DataBaseName = reader["DataBaseName"].ToString()
                        });
                    }
                }
            }
            return databases;
        }

        public List<Tables> GetTables(int clientId, int projectId, int databaseId)
        {
            var tables = new List<Tables>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT TableID, TableName FROM Tables WHERE ClientID = @ClientID AND ProjectID = @ProjectID AND DataBaseID = @DataBaseID AND IsActive = 1", connection);
                command.Parameters.AddWithValue("@ClientID", clientId);
                command.Parameters.AddWithValue("@ProjectID", projectId);
                command.Parameters.AddWithValue("@DataBaseID", databaseId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(new Tables
                        {
                            TableID = (long)reader["TableID"],
                            TableName = reader["TableName"].ToString()
                        });
                    }
                }
            }
            return tables;
        }

        public bool InsertDatabase(Database database)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spi_database", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DatabaseName", database.DataBaseName);
                cmd.Parameters.AddWithValue("@ClientID", database.ClientID);
                cmd.Parameters.AddWithValue("@ProjectID", database.ProjectID);
                int result = cmd.ExecuteNonQuery();

                return result > 0;
            }
        }

        public bool InsertTable(Tables table, int CreatedBy)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spi_table", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TableName", table.TableName);
                cmd.Parameters.AddWithValue("@ClientID", table.ClientID);
                cmd.Parameters.AddWithValue("@ProjectID", table.ProjectID);
                cmd.Parameters.AddWithValue("@DataBaseID", table.DataBaseID);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                int result = cmd.ExecuteNonQuery();

                return result > 0;
            }
        }
        public bool InsertProcedure(Procedures procedure, int CreatedBy)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spi_procedure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SPName", procedure.SPName);
                cmd.Parameters.AddWithValue("@ClientID", procedure.ClientID);
                cmd.Parameters.AddWithValue("@ProjectID", procedure.ProjectID);
                cmd.Parameters.AddWithValue("@DataBaseID", procedure.DataBaseID);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                int result = cmd.ExecuteNonQuery();

                return result > 0;
            }
        }

        public bool InsertIndex(Indexes Index, int CreatedBy)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spi_index", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IndexName", Index.IndexName);
                cmd.Parameters.AddWithValue("@ClientID", Index.ClientID);
                cmd.Parameters.AddWithValue("@ProjectID", Index.ProjectID);
                cmd.Parameters.AddWithValue("@DataBaseID", Index.DataBaseID);
                cmd.Parameters.AddWithValue("@TableID", Index.TableID);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                int result = cmd.ExecuteNonQuery();

                return result > 0;
            }
        }

        public bool InsertClient(Client client)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spi_client", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClientName", client.ClientName);
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
        public bool InsertProject(Claysys_SQLTask.Models.Project project)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("spi_project", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClientID", project.ClientID);
                cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public List<Procedures> GetProcedures()
        {
            List<Procedures> procedures = new List<Procedures>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Sps_procedureList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Procedures procedure = new Procedures();
                        procedure.SPID = Convert.ToInt32(reader["SPID"]);
                        procedure.SPName = reader["SPName"].ToString();
                        procedures.Add(procedure);
                    }
                    con.Close();
                }
            }
            return procedures;
        }

        public SpReview GetProcedureById(int SPID)
        {
            SpReview spReview = new SpReview();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Sps_SpReview", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SPID", SPID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        spReview.SPID = Convert.ToInt32(reader["SPID"]);
                        spReview.SPName = reader["SPName"].ToString();
                        spReview.ClientName = reader["ClientName"].ToString();
                        spReview.ProjectName = reader["ProjectName"].ToString();
                        spReview.DatabaseName = reader["DatabaseName"].ToString();

                    }
                    con.Close();
                }
            }
            return spReview;
        }

        public bool InsertSPReview(SpReview spReview, int CreatedBy)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("Spi_SpReview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SPID", spReview.SPID);
                cmd.Parameters.AddWithValue("@Description", spReview.Description);
                cmd.Parameters.AddWithValue("@AssignedBy", spReview.AssignedBy);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public List<SpReview> GetReviewsById(int SPID)
        {
            List<SpReview> spReviews = new List<SpReview>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sps_ProcedureReviews", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SPID", SPID);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SpReview spReview = new SpReview();
                        spReview.Id = Convert.ToInt32(reader["Id"]);
                        spReview.SPName = reader["SPName"].ToString();
                        spReview.Description = reader["Description"].ToString();
                        spReview.AssignedBy = reader["AssignedBy"].ToString();
                        spReview.EmployeeName = reader["EmployeeName"].ToString();
                        spReview.Created = Convert.ToDateTime(reader["Created"]);
                        spReview.IsMovedQA = Convert.ToBoolean(reader["QA"]);
                        spReview.IsMovedUAT = Convert.ToBoolean(reader["UAT"]);
                        spReview.ISMovedPROD = Convert.ToBoolean(reader["Production"]);

                        spReviews.Add(spReview);
                    }
                    con.Close();
                }
            }
            return spReviews;
        }

        public Homemodel GetHomeModelByUser(int userId)
        {
            var homeModel = new Homemodel
            {
                spnames = new List<Spname>(),
                availabilities = new List<Availability>(),
                tabels = new List<Tabel>(),
            };

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SPS_GetallSps", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            homeModel.spnames.Add(new Spname
                            {
                                Title = reader["Spname"].ToString()
                            });

                            if (reader.NextResult())
                            {
                                // Read Tabels

                            }
                        }
                    }
                }
                using (var commandSp = new SqlCommand("SPS_GetAvailability", connection))
                {
                    commandSp.CommandType = CommandType.StoredProcedure;
                    commandSp.Parameters.AddWithValue("@UserId", userId);
                    using (var Spreader = commandSp.ExecuteReader())
                    {
                        while (Spreader.Read())
                        {

                            homeModel.availabilities.Add(new Availability
                            {
                                Name = Spreader["UserName"].ToString(),
                                Priority = Spreader["PriorityType"].ToString()

                            });

                        }
                    }
                }
                //using (var commandTbl = new SqlCommand("sps_techstackuserview", connection))
                //{
                //    commandTbl.CommandType = CommandType.StoredProcedure;
                //    commandTbl.Parameters.AddWithValue("@UserId", userId);
                //    using (var reader = commandTbl.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {
                //            if (reader.Read())
                //            {
                //                homeModel.tabels.Add(new Tabel
                //                {
                //                    Title = reader["TableName"].ToString()
                //                });
                //            }
                //        }
                //    }
                //}
                using (var commandProjectQuery = new SqlCommand("sps_getCurrentproject", connection))
                {
                    commandProjectQuery.CommandType = CommandType.StoredProcedure;
                    commandProjectQuery.Parameters.AddWithValue("@UserId", userId);
                    using (var PQreader = commandProjectQuery.ExecuteReader())
                    {
                        while (PQreader.Read())
                        {

                            homeModel.Client = PQreader["Client"].ToString();
                            homeModel.Project = PQreader["Project"].ToString();
                            homeModel.Database = PQreader["Database"].ToString();

                        }
                    }
                }

            }

            return homeModel;
        }


        public DataTable GetSpDetails(string filters, int page = 1, int pageSize = 5)
        {
            var dataTable = new DataTable();
            
            List<HomeContentModel.FilterObject> filterList = new List<HomeContentModel.FilterObject>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterList = JsonConvert.DeserializeObject<List<HomeContentModel.FilterObject>>(filters);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sps_GetProcedureDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add pagination parameters
                    command.Parameters.AddWithValue("@Page", page);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    // Add filter parameters
                    foreach (var filter in filterList)
                    {
                        if (!string.IsNullOrEmpty(filter.Value))
                        {
                            command.Parameters.AddWithValue($"@{filter.Column}", filter.Value);
                        }
                        else
                        {
                            // Pass null or default value if the filter value is empty
                            command.Parameters.AddWithValue($"@{filter.Column}", DBNull.Value);
                        }
                    }

                    var adapter = new SqlDataAdapter(command);
                    connection.OpenAsync();
                    adapter.Fill(dataTable);
                }

            }
            return dataTable;
        }

        public int GetSpCount(string filters, int page = 1, int pageSize = 5)
        {
            var totalRecords = 0;
            List<HomeContentModel.FilterObject> filterList = new List<HomeContentModel.FilterObject>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterList = JsonConvert.DeserializeObject<List<HomeContentModel.FilterObject>>(filters);
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open(); 

                    using (var command = new SqlCommand("sps_Spdetails_TotalCount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add pagination parameters
                        command.Parameters.AddWithValue("@Page", page);
                        command.Parameters.AddWithValue("@PageSize", pageSize);

                        // Add filter parameters
                        foreach (var filter in filterList)
                        {
                            if (!string.IsNullOrEmpty(filter.Value))
                            {
                                command.Parameters.AddWithValue($"@{filter.Column}", filter.Value);
                            }
                            else
                            {
                                command.Parameters.AddWithValue($"@{filter.Column}", DBNull.Value);
                            }
                        }

                        totalRecords = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
             
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
               
                throw;
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return totalRecords;
        }

        public DataTable GetIndexDetails(string filters, int page = 1, int pageSize = 5)
        {
            var dataTable = new DataTable();

            List<HomeContentModel.FilterObject> filterList = new List<HomeContentModel.FilterObject>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterList = JsonConvert.DeserializeObject<List<HomeContentModel.FilterObject>>(filters);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sps_GetINDEXDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add pagination parameters
                    command.Parameters.AddWithValue("@Page", page);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    // Add filter parameters
                    foreach (var filter in filterList)
                    {
                        if (!string.IsNullOrEmpty(filter.Value))
                        {
                            command.Parameters.AddWithValue($"@{filter.Column}", filter.Value);
                        }
                        else
                        {
                            // Pass null or default value if the filter value is empty
                            command.Parameters.AddWithValue($"@{filter.Column}", DBNull.Value);
                        }
                    }

                    var adapter = new SqlDataAdapter(command);
                    connection.OpenAsync();
                    adapter.Fill(dataTable);
                }

            }
            return dataTable;
        }

        public int GetIndexCount(string filters, int page = 1, int pageSize = 5)
        {
            var totalRecords = 0;
            List<HomeContentModel.FilterObject> filterList = new List<HomeContentModel.FilterObject>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterList = JsonConvert.DeserializeObject<List<HomeContentModel.FilterObject>>(filters);
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("sps_GetindexDetailsCount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add pagination parameters
                        command.Parameters.AddWithValue("@Page", page);
                        command.Parameters.AddWithValue("@PageSize", pageSize);

                        // Add filter parameters
                        foreach (var filter in filterList)
                        {
                            if (!string.IsNullOrEmpty(filter.Value))
                            {
                                command.Parameters.AddWithValue($"@{filter.Column}", filter.Value);
                            }
                            else
                            {
                                command.Parameters.AddWithValue($"@{filter.Column}", DBNull.Value);
                            }
                        }

                        totalRecords = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (SqlException sqlEx)
            {

                Console.WriteLine($"SQL Error: {sqlEx.Message}");

                throw;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return totalRecords;
        }


        public DataTable GetClientDetails(string filters, int page = 1, int pageSize = 5)
        {
            var dataTable = new DataTable();

            List<HomeContentModel.FilterObject> filterList = new List<HomeContentModel.FilterObject>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterList = JsonConvert.DeserializeObject<List<HomeContentModel.FilterObject>>(filters);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("sps_GetClientDetails", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add pagination parameters
                    command.Parameters.AddWithValue("@Page", page);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    // Add filter parameters
                    foreach (var filter in filterList)
                    {
                        if (!string.IsNullOrEmpty(filter.Value))
                        {
                            command.Parameters.AddWithValue($"@{filter.Column}", filter.Value);
                        }
                        else
                        {
                            // Pass null or default value if the filter value is empty
                            command.Parameters.AddWithValue($"@{filter.Column}", DBNull.Value);
                        }
                    }

                    var adapter = new SqlDataAdapter(command);
                    connection.OpenAsync();
                    adapter.Fill(dataTable);
                }

            }
            return dataTable;
        }

        public int GetClientCount(string filters, int page = 1, int pageSize = 5)
        {
            var totalRecords = 0;
            List<HomeContentModel.FilterObject> filterList = new List<HomeContentModel.FilterObject>();

            if (!string.IsNullOrEmpty(filters))
            {
                filterList = JsonConvert.DeserializeObject<List<HomeContentModel.FilterObject>>(filters);
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("sps_GetClientDetailscount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add pagination parameters
                        command.Parameters.AddWithValue("@Page", page);
                        command.Parameters.AddWithValue("@PageSize", pageSize);

                        // Add filter parameters
                        foreach (var filter in filterList)
                        {
                            if (!string.IsNullOrEmpty(filter.Value))
                            {
                                command.Parameters.AddWithValue($"@{filter.Column}", filter.Value);
                            }
                            else
                            {
                                command.Parameters.AddWithValue($"@{filter.Column}", DBNull.Value);
                            }
                        }

                        totalRecords = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (SqlException sqlEx)
            {

                Console.WriteLine($"SQL Error: {sqlEx.Message}");

                throw;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return totalRecords;
        }

    }
}
