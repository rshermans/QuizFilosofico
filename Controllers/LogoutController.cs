using Microsoft.AspNetCore.Mvc;

namespace QuizFilosofico.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            //HttpContext.Session.SetString("JOGADOR", "");
            //HttpContext.Session.SetString("ESTADO", "");
            //HttpContext.Session.SetString("ADMINISTRADOR", "");

            //return Redirect("~/Home/Index");
            if (HttpContext.Session.GetString("ESTADO") == "ATIVO")
            {
                HttpContext.Session.SetString("JOGADOR", "");
                HttpContext.Session.SetString("ESTADO", "");
                HttpContext.Session.SetString("ADMINISTRADOR", "");

                return Redirect("~/Login/Index");

            }
            HttpContext.Session.SetString("JOGADOR", "");
                HttpContext.Session.SetString("ESTADO", "");
                HttpContext.Session.SetString("ADMINISTRADOR", "");

                //return PartialView("_LogoutConfirmation");
                return Redirect("~/Home/Index");
          
        }
    }
}
