namespace Claysys_SQLTask.Models
{
    public class ProcedureIndexRelation
    {
        public int Id { get; set; }

        public int ClientID { get; set; }

        public string ClientName { get; set; }

        public int ProjectID { get; set; }

        public string ProjectName { get; set; }

        public int DataBaseID { get; set; }

        public string DataBaseName { get; set; }

        public int SPID { get; set; }

        public string SPName { get; set; }

        public string IndexIds { get; set; }

        public bool IsActive { get; set; }

        public int TableID { get; set; }

        public string TableName { get; set; }

        public IEnumerable<int> SelectedIndexIds { get; set; }
    }
}
