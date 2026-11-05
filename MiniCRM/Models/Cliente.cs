namespace MiniCRM.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Cliente : BaseModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O documento é obrigatório.")]
        [RegularExpression(@"^\d{11}$|^\d{14}$", ErrorMessage = "Informe um CPF (11 dígitos) ou CNPJ (14 dígitos) válido.")]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Telefone inválido.")]
        public string Telefone { get; set; } = string.Empty;

        // Relacionamento
        public virtual ICollection<Campanha> Campanhas { get; set; } = new List<Campanha>();
    }
}
