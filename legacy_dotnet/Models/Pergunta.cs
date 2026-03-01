using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizFilosofico.Models;

public class Pergunta
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O enunciado da pergunta é obrigatório.")]
    [MaxLength(500, ErrorMessage = "O enunciado da pergunta não pode ter mais que 500 caracteres.")]
    public string? Enunciado { get; set; }

    [Required(ErrorMessage = "O nível da pergunta é obrigatório.")]
    [Range(1, 3, ErrorMessage = "O nível da pergunta deve estar entre 1 e 3.")]
    public int Nivel { get; set; }
    public int QuizzId { get; set; }
    [ForeignKey("QuizzId")]
    public virtual Quizz? Quizz { get; set; }

    public virtual ICollection<ItemDaPergunta>? ItemDaPerguntas { get; set; }
}


