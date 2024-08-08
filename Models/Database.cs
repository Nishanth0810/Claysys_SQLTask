namespace Claysys_SQLTask.Models
{
    public class Database
    {
        public int DataBaseID { get; set; }
        public string DataBaseName { get; set; }
        public int ClientID { get; set; }
        public int ProjectID { get; set; }
        public bool IsActive { get; set; }
    }
}
