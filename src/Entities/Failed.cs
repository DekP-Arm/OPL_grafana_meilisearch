using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace testAPI.src.Entities
{
    [Table("Failed")]
    public class FailedDbo
    {
        [Key]
        [Required]
        [Column("UserId", TypeName = "int")]
        public int UserId { get; set; }

        [Column("Username", TypeName = "nvarchar(255)")]
        public string? Username { get; set; }

        [Column("Password", TypeName = "nvarchar(255)")]
        public string? Password { get; set; }
    }
}