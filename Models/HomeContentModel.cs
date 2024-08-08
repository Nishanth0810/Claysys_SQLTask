namespace Claysys_SQLTask.Models
{
    public class HomeContentModel
    {
        public class Spname
        {
            public string Title { get; set; }
        }

        public class Availability
        {
            public string Name { get; set; }
            public string Priority { get; set; }
        }

        public class Tabel
        {
            public string Title { get; set; }
        }

        public class Homemodel { 
        
           public List<Spname> spnames { get; set; }
            public string Client { get; set; }
            public string Project { get; set; }
            public string Database { get; set; }
            public List<Availability> availabilities { get; set; }
            public List<Tabel> tabels { get; set; }

        }
        public class FilterObject
        {
            public string Column { get; set; }
            public string Value { get; set; }
        }
    }
}
