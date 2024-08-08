using Microsoft.AspNetCore.Http.HttpResults;

namespace Claysys_SQLTask.Models
{
    public class Indexes
    {
        public long IndexID {  get; set; }

        public string IndexName { get; set; }

        public int ClientID { get; set; }

        public int ProjectID { get; set; }

        public int DataBaseID { get; set; }

        public int TableID { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
