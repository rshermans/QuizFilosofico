using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizFilosofico.Data;
using QuizFilosofico.Models;
using System.Security.Permissions;

namespace QuizFilosofico.Controllers;

public class Apoio : Controller
{

    private readonly ApplicationDbContext _context;

    public Apoio(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {

        return View();
    }

    public IActionResult Ranking(int? SEUJOGADOR)
    {
        
        List<Partida> partidas;

        if (SEUJOGADOR == 0 || SEUJOGADOR == null)
        {
            partidas = _context.Partidas
                .Include(p => p.Jogador)
                .Include(p => p.Quizz)
                .OrderByDescending(p => p.Pontuacao)
                .Take(10)
                .ToList();
            ViewBag.Partidas = new SelectList(_context.Jogadores, "Id", "Nome");
            //ViewBag.SEUJOGADORID = (int)SEUJOGADOR;
            return View(partidas);
           
        }
        else
        {
            partidas = _context.Partidas
                .Include(p => p.Jogador)
                .Include(p => p.Quizz)
                .Where(p => p.Jogador.Id == SEUJOGADOR)
                .ToList();
            ViewBag.Partidas = new SelectList(_context.Jogadores, "Id", "Nome");
            //ViewBag.SEUJOGADORID = SEUJOGADOR;
            return View(partidas);
          

        }
      
    }


    public IActionResult Niveis()
    { 
        return View();
    }


    public IActionResult Referencias()
    {
        return View();
    }


}
