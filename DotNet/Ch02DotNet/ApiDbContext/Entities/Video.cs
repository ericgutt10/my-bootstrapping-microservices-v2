using System.ComponentModel.DataAnnotations;

namespace ApiHost.Entities
{
    public class Video(string title)
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Title { get; set; } = title;

        [MaxLength(1000)]
        public string? Path { get; set; }
    }
}
