using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuizFilosofico.Data;
using QuizFilosofico.Models;
using System.Collections.Generic;
using System.Linq;


namespace QuizFilosofico.Controllers;

public class Quiz : Controller
{
    private readonly ApplicationDbContext _context;
    private int i; //Variavel para necessidades de interação  e controle

    public Quiz(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Quizz()
    {
        if (   //05/06/2023 - Realizado a criação das seesões
                //Aqui são as váriaveis de sessão que controlam o acesso a pagina
            
                HttpContext.Session.GetString("ESTADO") == "ATIVO" &&
                HttpContext.Session.GetString("JOGADOR") != "")
        {
            
        //enviar os quizzes para a drop na view

        ViewBag.TEMA = new SelectList(_context.Quizzs, "Id", "Tema");

        //Enviar os niveis de forma distintas para a dropdown da View
        var niveisDistintos = _context.Perguntas.Select(p => p.Nivel).Distinct().ToList();
        ViewBag.Y = new SelectList(niveisDistintos, "Nivel");
            // Método para obter os quizzes da base de dados

            //List<Quiz> quizzes = 
            ViewBag.QUIZ = _context.Quizzs.Include(q => q.Perguntas).ToList(); //.ThenInclude(p => p.ItemDaPerguntas).Include(q => q.Partidas).ToList();

            //Partidas do Jogador Logado
            var atual = HttpContext.Session.GetString("JOGADOR");
            int JogadorAtual = _context.Jogadores.FirstOrDefault(j => j.Nome == atual).Id;
            int pontuacaoTotal = _context.Partidas.Where(p => p.JogadorId == JogadorAtual).Sum(p => p.Pontuacao);
            ViewBag.Pontuacao = pontuacaoTotal;
            return View();

            }
        else
        {
            //Por opção, sem acesso redirecionamos para o controlador Login
            return Redirect("~/Login/Index");
        }
    }  

    /// <summary>
    /// Action responsável por exibir as questões do Quiz Filosófico.
    /// </summary>
    /// <param name="tema">O id do tema selecionado.</param>
    /// <param name="nivel">O id do nível selecionado.</param>
    /// <returns>A View com as questões do Quiz Filosófico.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Questoes(int? tema, int? nivel)
    {
        // Verifica se o tema e o nível foram selecionados
        if (tema == null || tema == 0 || nivel == null || nivel == 0)
        {
            ViewBag.Erro = "Você deve selecionar o tema e o nível do Quiz Filosófico!";
            //return View();
        }

        // Cria um objeto Random para gerar números aleatórios
            var random = new Random();
        //Variavel sem include, criada para opção do programador
            var perguntas = _context.Perguntas.Where(n => n.Nivel == nivel && n.QuizzId == tema).ToList();

        //Este Bag leva as Perguntas para a View
        ViewBag.PerguntaX = _context.Perguntas
                .Where(n => n.Nivel == nivel && n.QuizzId == tema)
                .AsEnumerable() // Materializa a consulta e traz os resultados para a memória
                .OrderBy(p => random.Next()) // Ordena as perguntas por um número aleatório no lado do cliente
                .Take(3) // Pega as 3 primeiras perguntas da sequência ordenada
                .ToList();
        // Este Bag leva os itens das perguntas para a view
        ViewBag.ItemDaPergunta = _context.ItemDaPerguntas
            .AsEnumerable() // Materializa a consulta e traz os resultados para a memória
            .OrderBy(p => random.Next())// Ordena os itens da Pergunta por um número aleatório no lado do cliente
            .ToList();
            // Armazenar o valor de tema na variável de sessão
            // Inserção do ?? para previnir em caso de tema = null, inserir o default 0 
            HttpContext.Session.SetInt32("TEMA", tema ?? 0);
            HttpContext.Session.SetInt32("NIVEL", nivel ?? 0);

        return View();
    }


    public IActionResult Resultado(IFormCollection formValues)
    {
        int resultado = 0; // Variavel para o resultado
        var nivel = HttpContext.Session.GetInt32("NIVEL");// Variável para o nível do quiz
        List<int> corretas = new List<int>(); // Lista para receber os itens corretos da BD
        corretas =_context.ItemDaPerguntas.Where(c => c.IsCorrect == true).Select(c=> c.Id).ToList(); //Itens das perguntas corretos

        //Ciclo para verificação das  respostas do formulario
        foreach (var item in formValues)
        {
            //Verificação das corretas
            foreach (var itemCorreto in corretas)
            {
                 string R = itemCorreto.ToString();
                if (R == item.Value )
                { 
                        resultado += (int)nivel; // Soma de pontos multiplicados pelo nível

                }
            }
            


        }

        if (resultado < 3)
        {
            ViewBag.Mensagem = "Você precisa melhorar para seguir para o próximo nível!";
        }
        else { 
        ViewBag.Mensagem = "Você está pronto para o próximo nível do Quiz Filosofico";} 

        ViewBag.Resultado = resultado.ToString(); //Envio do Resultado para a View
        ViewBag.QuizID = HttpContext.Session.GetInt32("TEMA");
        GravarPartida(resultado);
       
        return View();
    }


    public ActionResult<Partida> GravarPartida(int pontuacao)
    {
        string nomeJogador = HttpContext.Session.GetString("JOGADOR");

        // Consultar o jogador pelo nome
        Jogador jogador = _context.Jogadores.FirstOrDefault(j => j.Nome == nomeJogador);
        //Partida partidaatual = new Partida();
        //partidaatual.QuizzId = (int)HttpContext.Session.GetInt32("TEMA");
        // Armazenar o ID do Jogador na variável de sessão
        HttpContext.Session.SetInt32("IDJOGADOR", jogador.Id);

        if (jogador != null)
        {
            int jogadorId = jogador.Id;

            // Criar um objeto Partida
            Partida partida = new Partida
            {
                Data = DateTime.Now,
                Pontuacao = pontuacao,
                JogadorId = jogadorId,
                // Defina o valor do QuizzId conforme necessário
                QuizzId = (int)HttpContext.Session.GetInt32("TEMA")
        };

            // Adicionar a partida ao contexto do banco de dados
            _context.Partidas.Add(partida);

            // Salvar as alterações no banco de dados
            _context.SaveChanges();

            // Redirecionar para uma ação ou exibir uma mensagem de sucesso
            return partida;
        }

        // Se não for possível encontrar o jogador, redirecionar ou exibir uma mensagem de erro
        return BadRequest("Jogador não encontrado");
    }



}



