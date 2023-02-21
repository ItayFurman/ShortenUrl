using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProjectUrlShort.Models
{

    public class URL
    {
        [Required]
        public string FullUrl { get; set; }
        [Required]
        [Key]
        public string ShortUrl { get; set; }
        [Required]
        public int NumOfRequest { get; set; }
        [Required]
        public DateTime Time { get; set; }
        public IdentityUser UserUrl { get; set; }
        public URL()
        {
            Time= DateTime.Now;
        }
    }
}
