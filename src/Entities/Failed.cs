using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OPL_grafana_meilisearch.src.Entities
{
    [Table("Failed")]
    public class FailedDbo
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