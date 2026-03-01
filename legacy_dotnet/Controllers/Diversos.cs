using Microsoft.AspNetCore.Mvc;
using QuizFilosofico.Data;
using QuizFilosofico.Models;
using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;
using QuizFilosofico.Models.Extensions;

namespace QuizFilosofico.Controllers
{
    public class Diversos : Controller
    {
        private readonly ApplicationDbContext _context;
        private JogoDaForca jogoDaForca;
        public Diversos (ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Atividade1()
        {

            return View();
        }

        public IActionResult IniciarJogo()
        {
            string palavra = ObterPalavraAleatoria();
            int tentativasRestantes = 6; // Definir o número de tentativas iniciais
            List<string> dicas = new List<string>
        {
            "É uma linguagem de programação",
            "É utilizada no desenvolvimento de websites",
            "É executado em um navegador",
            "É uma máquina eletrônica",
            "É um conjunto de instruções"
        };

                jogoDaForca = new JogoDaForca(palavra, tentativasRestantes, dicas);

                return RedirectToAction("JogoDaForca");
            }

        public IActionResult JogoDaForca()
        {
            if (jogoDaForca == null)
            {
                // Redirecionar para a página inicial do jogo
                return RedirectToAction("Index");
            }

            ViewBag.PalavraEscondida = jogoDaForca.GetPalavraEscondida();
            ViewBag.TentativasRestantes = jogoDaForca.GetTentativasRestantes();
            ViewBag.Dicas = jogoDaForca.GetDicas();

            return View();
        }

        public IActionResult ResultadoJogoDaForca()
        {
            if (jogoDaForca == null)
            {
                // Redirecionar para a página de início do jogo
                return RedirectToAction("Index");
            }

            if (jogoDaForca.PalavraCompleta())
            {
                ViewBag.Mensagem = "Parabéns! Você acertou a palavra.";
            }
            else
            {
                ViewBag.Mensagem = "Você perdeu! A palavra correta era: " + jogoDaForca.GetPalavraEscondida();
            }

            return View();
        }

        public IActionResult JogoCompletarPalavras(string resposta)
        {
            ViewBag.Palavra = PegarPalavrasNasPerguntas(10);
            return View(resposta);
        }



        //Metodos de apoio para as atividades dos diverso

        //Para separar as palavras de uma frase
        private List<string> SepararPalavras(string frase)
        {
            // Caracteres de separação das palavras
            char[] separadores = { ' ', ',', '.', '!', '?' };

            // Dividir a frase em palavras
            string[] palavras = frase.Split(separadores, StringSplitOptions.RemoveEmptyEntries);

            // Remover espaços em branco adicionais
            List<string> palavrasLimpa = new List<string>();
            foreach (string palavra in palavras)
            {
                string palavraLimpa = palavra.Trim();
                if (!string.IsNullOrEmpty(palavraLimpa))
                {
                    palavrasLimpa.Add(palavraLimpa);
                }
            }

            return palavrasLimpa;
        }

        //Para colocar uma falha na palavra, para torna-se incompleta

        private string IncompletarPalavra(string[] palavra)
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            foreach (string letra in palavra)
            {
                // Substitui algumas letras por espaço em branco ou underline
                if (random.Next(0, 2) == 0)
                {
                    sb.Append(letra);
                }
                else
                {
                    sb.Append("_");
                }
            }

            return sb.ToString();
        }

        //
        public List<Pergunta> PegarPalavrasNasPerguntas(int count)
        {
             var random = new Random();
                return _context.Perguntas.OrderBy(p => random.Next()).Take(count).ToList();
            
        }



        // Metodo para verificar palavra
        [HttpPost]
        public IActionResult AdivinharLetra(char letra)
        {
            if (jogoDaForca != null)
            {
                jogoDaForca.AdivinharLetra(letra);
            }

            return RedirectToAction("JogoDaForca");
        }
        
        private string ObterPalavraAleatoria()
        {
                List<string> palavras = new List<string>
        {
            "programacao",
            "desenvolvimento",
            "web",
            "computador",
            "software"
        };

                Random random = new Random();
                int index = random.Next(palavras.Count);
                return palavras[index];
        }



    }
}
