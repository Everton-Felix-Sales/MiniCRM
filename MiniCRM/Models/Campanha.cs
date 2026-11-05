namespace MiniCRM.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Campanha : BaseModel
    {
        [Required(ErrorMessage = "O nome da campanha é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Range(0.01, 1000000.00, ErrorMessage = "O orçamento deve ser maior que zero.")]
        public decimal Orcamento { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }

        [Required]
        public string Status { get; set; } = "Planejada"; // Planejada, Em Andamento, Concluída

        // Chave Estrangeira
        [Required(ErrorMessage = "Selecione o cliente.")]
        public Guid ClienteId { get; set; }
        public virtual Cliente? Cliente { get; set; }
    }
}
