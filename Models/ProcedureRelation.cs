namespace Claysys_SQLTask.Models
{
    public class ProcedureRelation
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

        public int IndexID { get; set; }

        public string IndexName { get; set; }

        public bool IsActive { get; set; }

        public int TableID { get; set; }

        public string TableName { get; set; }

        public IEnumerable<int> SelectedIndexIds { get; set; }
        public string TableIds { get; set; }

        public IEnumerable<int> SelectedTableIds { get; set; }
    }
}
