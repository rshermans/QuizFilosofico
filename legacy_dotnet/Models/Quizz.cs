using QuizFilosofico.Models.Validacao;
using System.ComponentModel.DataAnnotations;

namespace QuizFilosofico.Models;

public class Quizz
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "O tema é obrigatório.")]
    [StringLength(200, ErrorMessage = "O tema deve ter no máximo 200 caracteres.")]
    public string Tema { get; set; }


    //[SeExisteImagemNoFicheiro(ErrorMessage = "A imagem não existe no servidor.")]
    public string? ImgCaminho { get; set; }
    public virtual List<Pergunta>? Perguntas { get; set; }
    public virtual List<Partida>? Partidas { get; set; }


}