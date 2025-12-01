namespace QuizFilosofico.Models.OpenAI;

public class OpenAiOptions
{
    public string? ApiKey { get; set; }

    /// <summary>
    /// Nome do modelo a ser usado na API de chat.
    /// </summary>
    public string Model { get; set; } = "gpt-3.5-turbo";

    /// <summary>
    /// Número padrão de perguntas a gerar quando não for especificado.
    /// </summary>
    public int DefaultQuestionCount { get; set; } = 5;
}
