using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizFilosofico.Data;
using QuizFilosofico.Models.Requests;
using QuizFilosofico.Services;

namespace QuizFilosofico.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OpenAiQuizController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly OpenAiQuizService _openAiQuizService;

    public OpenAiQuizController(ApplicationDbContext context, OpenAiQuizService openAiQuizService)
    {
        _context = context;
        _openAiQuizService = openAiQuizService;
    }

    [HttpPost("gerar")]
    public async Task<IActionResult> Generate([FromBody] GenerateQuizRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var quantidade = request.QuantidadePerguntas ?? _openAiQuizService.DefaultQuestionCount;

        var quiz = await _openAiQuizService.GenerateQuizAsync(request.Tema, quantidade, request.Descricao);
        if (quiz == null)
        {
            return StatusCode(StatusCodes.Status502BadGateway, "Não foi possível gerar o quiz através da OpenAI.");
        }

        _context.Quizzs.Add(quiz);
        await _context.SaveChangesAsync();

        await _context.Entry(quiz).Collection(q => q.Perguntas!).LoadAsync();
        foreach (var pergunta in quiz.Perguntas ?? Enumerable.Empty<Models.Pergunta>())
        {
            await _context.Entry(pergunta).Collection(p => p.ItemDaPerguntas!).LoadAsync();
        }

        return CreatedAtAction(
            actionName: "Details",
            controllerName: "Quizzs",
            routeValues: new { id = quiz.Id },
            value: new
            {
                quiz.Id,
                quiz.Tema,
                quiz.Descricao,
                Perguntas = quiz.Perguntas?.Select(p => new
                {
                    p.Id,
                    p.Enunciado,
                    p.Nivel,
                    Opcoes = p.ItemDaPerguntas?.Select(o => new { o.Id, Texto = o.Item, o.IsCorrect })
                })
            });
    }
}
