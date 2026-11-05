using System.ComponentModel.DataAnnotations;

namespace MiniCRM.Models
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAtualizacao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
