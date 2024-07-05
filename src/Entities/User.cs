using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace testAPI.src.Entities
{
    [Table("USER")]
    public class UserDbo
    {
        [Key]
        [Required]
        [Column("userid", TypeName = "int")]
        public int UserId { get; set; }

        [Column("username", TypeName = "nvarchar(255)")]
        public string? Username { get; set; }

        [Column("password", TypeName = "nvarchar(255)")]
        public string? Password { get; set; }
    }
}