using System.ComponentModel.DataAnnotations;

namespace QuizFilosofico.Models.Requests;

public class GenerateQuizRequest
{
    [Required]
    [StringLength(200)]
    public string Tema { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descricao { get; set; }

    [Range(1, 10, ErrorMessage = "A quantidade de perguntas deve estar entre 1 e 10.")]
    public int? QuantidadePerguntas { get; set; }
}
