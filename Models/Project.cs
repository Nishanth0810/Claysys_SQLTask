namespace Claysys_SQLTask.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ClientID { get; set; }
        public bool IsActive { get; set; }
    }
}
