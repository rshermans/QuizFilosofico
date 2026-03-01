using Microsoft.AspNetCore.Mvc;

namespace QuizFilosofico.Controllers
{
    public class Organizacao : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("ADMINISTRADOR") == "SIM" &&
                HttpContext.Session.GetString("ESTADO") == "ATIVO" &&
                HttpContext.Session.GetString("JOGADOR") != "")
            { 
                ViewBag.Title = "Organização - Quiz Filosófico";
            return View();
            }
            else
            {
                return Redirect("~/Login/Index");
            } 
        }
    }
}
