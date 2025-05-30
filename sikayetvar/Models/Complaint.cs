using System;
using System.ComponentModel.DataAnnotations;

namespace sikayetvar.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(250, ErrorMessage = "Başlık en fazla 250 karakter olabilir")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur")]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}

