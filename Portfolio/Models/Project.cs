using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class Project
    {
        // A unique idenfication for a Project.
        public int Id { get; set; }

        // The name of the Project.
        public string? Name { get; set; }

        // A description of the purpose of the Project. 
        public string? Description { get; set; }

        // Indicates the type of application. Web , Mobile or Desktop.
        public string? Type { get; set; }

        // A url for the icon of the project.
        public string? ImageUrl { get; set; }

        // Indicates whether this project is currently in development.
        public string? IsInDevelopment {  get; set; }    // Y - Yes or N - No

        // Indicates the date at which the project was started.
        [DataType(DataType.Date)]
        public DateTime DateStarted { get; set; }

        // Indicates the date at which the project was released.
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        // Stores a link to a github repo of this project.
        public string GitHubRepo { get; set; }
    }
}
