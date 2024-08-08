namespace Claysys_SQLTask.Models
{
    public class Tables
    {
        public long TableID { get; set; }

        public string TableName { get; set; }

        public int ClientID { get; set; }

        public int ProjectID { get; set; }

        public int DataBaseID { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }
    }
}
