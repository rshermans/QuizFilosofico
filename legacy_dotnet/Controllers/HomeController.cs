using Microsoft.AspNetCore.Mvc;
using QuizFilosofico.Data;

namespace QuizFilosofico.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext dbp;
    public HomeController(ApplicationDbContext context)
    {
        dbp = context;
    }


    public IActionResult Index()
    {

        return View();
    }

    //[Area("Quiz")]
    //public IActionResult GetQuizOptions()
    //{
    //    var quizz = dbp.Quizzs.ToList();// carregue as opções do quiz do banco de dados (substitua '_context' pelo contexto do seu aplicativo)
    //    return PartialView("_QuizOptions", quizz); // retorne a View parcial '_QuizOptions' passando as opções do quiz como modelo
    //}




}


