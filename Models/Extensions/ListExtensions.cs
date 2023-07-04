using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace QuizFilosofico.Models.Extensions
{
    public static class ListExtensions
    {
        // Cria um objeto Random para gerar números aleatórios
        private static readonly Random random = new Random();

        // Cria um método Shuffle que recebe uma lista e retorna uma lista embaralhada
        public static List<ItemDaPergunta> Shuffle<T>(this List<ItemDaPergunta> list)
        {
            // Ordena a lista por um número aleatório
            return list.OrderBy(x => random.Next()).ToList();
        }
    }
}
