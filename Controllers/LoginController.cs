using Microsoft.AspNetCore.Mvc;
using QuizFilosofico.Data;
using QuizFilosofico.Models;

namespace QuizFilosofico.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Back([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext.Request;

            if (request.Headers.ContainsKey("Referer"))
            {
                var refererUrl = request.Headers["Referer"].ToString();
                return Redirect(refererUrl);
            }

            // Caso o cabeçalho "Referer" não esteja presente, redirecione para uma ação específica
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index(string email, string password)
        {
            // Caso o login não tenha sido invocado de lado nenhum,
            // faz login e vai para a página pública ou inicial
            if (HttpContext.Session.GetString("CONTROLADOR") == null)
                HttpContext.Session.SetString("CONTROLADOR", "Home");

            // Verificar se esse email existe na base de ados e
            // se a respectiva senha corresponde

            Jogador usu = new Jogador();
            usu = _context.Jogadores.FirstOrDefault(u => u.Email == email && u.Senha == password && u.Estado);

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("JOGADOR")))
            {
                // Redirecionar para uma página informando que o jogador já está logado
                return Redirect("~/Login/Jalogado");
            }
            if (usu == null)
            {
                HttpContext.Session.SetString("JOGADOR", "");
                HttpContext.Session.SetString("CONTROLADOR", "Home");

                return View();
            
            }
            else
            {
                if (usu.Estado == false)
                {
                    HttpContext.Session.SetString("JOGADOR", "");
                    HttpContext.Session.SetString("ESTADO", "");
                    HttpContext.Session.SetString("ADMINISTRADOR", "");
                   

                    return Redirect("~/Home/Index");
                }
                else
                {
                    HttpContext.Session.SetString("JOGADOR", usu.Nome);
                    HttpContext.Session.SetString("ESTADO", "ATIVO");

                    if (usu.Administrador == true)
                    {
                        HttpContext.Session.SetString("ADMINISTRADOR", "SIM");
                       
                    }
                  
                    return Redirect("~/" + HttpContext.Session.GetString("CONTROLADOR") + "/Index");
                }

            }

        }
        public IActionResult Jalogado()
        {
            return View();
        }
    }
}
