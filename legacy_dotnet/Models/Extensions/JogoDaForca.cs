using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace QuizFilosofico.Models.Extensions
{


    public class JogoDaForca
    {
        private string palavra;
        private List<char> letrasDescobertas;
        private int tentativasRestantes;
        private List<string> dicas;


        public JogoDaForca(string palavra, List<string> dicas, int tentativas)
        {
            this.palavra = palavra.ToLower();
            this.dicas = dicas;
            tentativasRestantes = tentativas;
            letrasDescobertas = new List<char>();
        }

        public JogoDaForca(string palavra, int tentativasRestantes, List<string> dicas)
        {
            this.palavra = palavra;
            this.tentativasRestantes = tentativasRestantes;
            this.dicas = dicas;
        }

        public bool IsFimDeJogo()
        {
            return tentativasRestantes <= 0 || PalavraCompleta();
        }

        public bool PalavraCompleta()
        {
            foreach (char letra in palavra)
            {
                if (!letrasDescobertas.Contains(letra))
                {
                    return false;
                }
            }
            return true;
        }

        public void AdivinharLetra(char letra)
        {
            letra = char.ToLower(letra);

            if (!char.IsLetter(letra))
            {
                return;
            }

            if (letrasDescobertas.Contains(letra))
            {
                return;
            }

            bool acertou = false;

            if (palavra.Contains(letra))
            {
                letrasDescobertas.Add(letra);
                acertou = true;
            }
            else
            {
                tentativasRestantes--;
            }
        }

        public string GetPalavraEscondida()
        {
            string palavraEscondida = "";
            foreach (char letra in palavra)
            {
                if (letrasDescobertas.Contains(letra))
                {
                    palavraEscondida += letra + " ";
                }
                else
                {
                    palavraEscondida += "_ ";
                }
            }
            return palavraEscondida.TrimEnd();
        }

        public int GetTentativasRestantes()
        {
            return tentativasRestantes;
        }

        public List<string> GetDicas()
        {
            return dicas;
        }
    }

}
