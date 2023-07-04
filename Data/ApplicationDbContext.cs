using Microsoft.EntityFrameworkCore;

using QuizFilosofico.Models;

namespace QuizFilosofico.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    //aqui:

    //public DbSet? TClientes { get; set; }

    //para a subapp CliSenhas:
    //public DbSet? TTipos { get; set; }
    //public DbSet? TItems { get; set; }

    public DbSet<Jogador> Jogadores { get; set; }
    public DbSet<Partida> Partidas { get; set; }
    public DbSet<Pergunta> Perguntas { get; set; }
    public DbSet<ItemDaPergunta> ItemDaPerguntas { get; set; }
    public DbSet<Quizz> Quizzs { get; set; }



}

