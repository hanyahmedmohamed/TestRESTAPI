using System.ComponentModel.DataAnnotations;

namespace TestRESTAPI3.Models
{
    public class dtoLogin
    {
        [Required]
        public string userName { get; set; }
        
        [Required]
        public string password { get; set; }
    }
}
