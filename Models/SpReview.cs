using Microsoft.AspNetCore.Http.HttpResults;

namespace Claysys_SQLTask.Models
{
    public class SpReview
    {
        public long Id { get; set; }

        public string EmployeeName { get; set; }

        public long SPID { get; set; }

        public string ClientName { get; set; }

        public string ProjectName { get; set; }

        public string DatabaseName { get; set; }

        public string SPName { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public string AssignedBy { get; set; }

        public int CreatedBy { get; set; }

        public bool IsValid { get; set; }

        public bool IsMovedQA { get; set; }

        public bool IsMovedUAT { get; set; }

        public bool ISMovedPROD { get; set; }

    }
}
