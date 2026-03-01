using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizFilosofico.Models;

public class ItemDaPergunta
{
    [Key]
    public int Id { get; set; }
    public string? Item { get; set; }

    public bool IsCorrect { get; set; }

    [Display(Name = "Pergunta")]
    public int PerguntaId { get; set; }
    [ForeignKey("PerguntaId")]
    public virtual Pergunta? Pergunta { get; set; }

}
