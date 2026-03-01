using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using QuizFilosofico.Models;
using QuizFilosofico.Models.OpenAI;

namespace QuizFilosofico.Services;

public class OpenAiQuizService
{
    private readonly HttpClient _httpClient;
    private readonly OpenAiOptions _options;
    private readonly ILogger<OpenAiQuizService> _logger;

    public OpenAiQuizService(HttpClient httpClient, IOptions<OpenAiOptions> options, ILogger<OpenAiQuizService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        _httpClient.BaseAddress ??= new Uri("https://api.openai.com/v1/");
    }

    public int DefaultQuestionCount => Math.Max(1, _options.DefaultQuestionCount);

    public async Task<Quizz?> GenerateQuizAsync(string tema, int questionCount, string? descricaoOverride)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("A chave da API da OpenAI não está configurada. Preencha OpenAI:ApiKey em appsettings.json ou nas variáveis de ambiente.");
        }

        var requestBody = new ChatRequest
        {
            Model = _options.Model,
            Temperature = 0.7,
            Messages = new List<ChatMessage>
            {
                new()
                {
                    Role = "system",
                    Content = "Gere quizzes em JSON válido. Estrutura: {\"descricao\":string,\"tema\":string,\"perguntas\":[{\"enunciado\":string,\"nivel\":1-3,\"opcoes\":[{\"texto\":string,\"correta\":true|false}]}]}. Use sempre Português."
                },
                new()
                {
                    Role = "user",
                    Content = $"Crie um quiz sobre {tema} com {questionCount} perguntas de múltipla escolha. Inclua apenas uma opção correta por pergunta."
                }
            }
        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "chat/completions")
        {
            Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        };
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

        var response = await _httpClient.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        var chatResponse = JsonSerializer.Deserialize<ChatResponse>(responseJson, SerializerOptions()) ?? new ChatResponse();
        var content = chatResponse.Choices.FirstOrDefault()?.Message?.Content;
        if (string.IsNullOrWhiteSpace(content))
        {
            _logger.LogWarning("Resposta da OpenAI não contém conteúdo utilizável: {Response}", responseJson);
            return null;
        }

        var sanitized = CleanContent(content);
        var payload = JsonSerializer.Deserialize<GeneratedQuizPayload>(sanitized, SerializerOptions());
        if (payload == null)
        {
            _logger.LogWarning("Não foi possível desserializar o conteúdo retornado: {Content}", sanitized);
            return null;
        }

        var quiz = new Quizz
        {
            Tema = payload.Tema ?? tema,
            Descricao = descricaoOverride ?? payload.Descricao ?? $"Quiz sobre {tema}",
            Perguntas = payload.Perguntas?.Select(question => new Pergunta
            {
                Enunciado = question.Enunciado,
                Nivel = NormalizeLevel(question.Nivel),
                ItemDaPerguntas = question.Opcoes?.Select(option => new ItemDaPergunta
                {
                    Item = option.Texto,
                    IsCorrect = option.Correta
                }).ToList()
            }).ToList()
        };

        return quiz;
    }

    private static int NormalizeLevel(int? nivel)
    {
        if (nivel is null or < 1)
        {
            return 1;
        }

        if (nivel > 3)
        {
            return 3;
        }

        return nivel.Value;
    }

    private static string CleanContent(string content)
    {
        var cleaned = content.Trim();
        if (cleaned.StartsWith("```") && cleaned.Contains('\n'))
        {
            var firstBreak = cleaned.IndexOf('\n');
            var lastFence = cleaned.LastIndexOf("```", StringComparison.Ordinal);
            if (firstBreak >= 0 && lastFence > firstBreak)
            {
                cleaned = cleaned[(firstBreak + 1)..lastFence];
            }
        }

        return cleaned.Trim();
    }

    private static JsonSerializerOptions SerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private sealed class ChatRequest
    {
        public string Model { get; set; } = "gpt-3.5-turbo";
        public double Temperature { get; set; }
        public IList<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }

    private sealed class ChatMessage
    {
        public string Role { get; set; } = "user";
        public string Content { get; set; } = string.Empty;
    }

    private sealed class ChatResponse
    {
        public List<Choice> Choices { get; set; } = new();
    }

    private sealed class Choice
    {
        public ChatMessage? Message { get; set; }
    }

    private sealed class GeneratedQuizPayload
    {
        public string? Descricao { get; set; }
        public string? Tema { get; set; }
        public List<GeneratedQuestionPayload>? Perguntas { get; set; }
    }

    private sealed class GeneratedQuestionPayload
    {
        public string? Enunciado { get; set; }
        public int? Nivel { get; set; }
        public List<GeneratedOptionPayload>? Opcoes { get; set; }
    }

    private sealed class GeneratedOptionPayload
    {
        public string? Texto { get; set; }
        public bool Correta { get; set; }
    }
}
