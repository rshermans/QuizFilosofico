using System.ComponentModel.DataAnnotations;

namespace QuizFilosofico.Models;

public class Partida
{
    [Key]
    public int Id { get; set; }

    //[Display(Name = "Data da Partida")]
    //[Column(TypeName = "datetime2")]
    //[DisplayFormat] //(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")

    [Display(Name = "Data da Partida")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Data { get; set; }

    [Display(Name = "Pontuação")]
    [Range(0, int.MaxValue, ErrorMessage = "A pontuação deve ser um número positivo.")]
    public int Pontuacao { get; set; }

    [Display(Name = "Jogador")]
    public int JogadorId { get; set; }
    
    public virtual Jogador Jogador { get; set; }

    public int QuizzId { get; set; }
    public virtual Quizz Quizz { get; set; }
}
