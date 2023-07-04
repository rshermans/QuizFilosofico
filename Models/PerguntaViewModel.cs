namespace QuizFilosofico.Models;

public class PerguntaViewModel
{
    public int Id { get; set; }
    public Pergunta? Enunciado { get; set; }
    public ItemDaPergunta? RespostaCorreta { get; set; }
    public List<ItemDaPergunta>? RespostasIncorretas { get; set; }

    public IEnumerable<Quizz>? Quizzs { get; set; }
    public IEnumerable<Pergunta>? Perguntas { get; set; }
    public IEnumerable<ItemDaPergunta>? ItemDaPerguntas { get; set; }
}
