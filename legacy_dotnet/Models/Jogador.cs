using System.ComponentModel.DataAnnotations;

namespace QuizFilosofico.Models;

public class Jogador
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O email não é válido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string? Senha { get; set; }

    //administrador sim ou não
    public bool Administrador { get; set; }
    //ativo ou inativo?
    public bool Estado { get; set; }

    //Coleção de partidas que ele participou
    public virtual ICollection<Partida>? Partidas { get; set; }
}

