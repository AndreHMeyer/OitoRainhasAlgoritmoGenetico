using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OitoRainhasAlgoritmoGenetico
{
    public class OitoRainhas
    {
        static Random random = new Random();

        public int TamanhoTabuleiro { get; set; } = 8;
        public int TamanhoPopulacao { get; set; } = 100;
        public int MaxGeracoes { get; set; } = 1000;
        public double TaxaCruzamento { get; set; } = 0.8;
        public double TaxaMutacao { get; set; } = 0.3;

        public OitoRainhas(int tamanhoTabuleiro, int tamanhoPopulacao, int maxGeracoes, double taxaCruzamento, double taxaMutacao)
        {
            if (tamanhoTabuleiro < 4)
                throw new ArgumentException("Não há soluções para tabuleiros menores que 4x4.");

            TamanhoTabuleiro = tamanhoTabuleiro;
            TamanhoPopulacao = tamanhoPopulacao;
            MaxGeracoes = maxGeracoes;
            TaxaCruzamento = taxaCruzamento;
            TaxaMutacao = taxaMutacao;
        }

        public int CalcularConflitos(int[] tabuleiro)
        {
            int conflitos = 0;

            for (int i = 0; i < tabuleiro.Length; i++)
            {
                for (int j = i + 1; j < tabuleiro.Length; j++)
                {
                    // Verifica onflito na mesma linha ou na mesma diagonal
                    if (tabuleiro[i] == tabuleiro[j] || Math.Abs(tabuleiro[i] - tabuleiro[j]) == Math.Abs(i - j))
                        conflitos++;
                }
            }

            return conflitos;
        }

        public int[] GerarTabuleiroAleatorio()
        {
            return Enumerable.Range(0, TamanhoTabuleiro).OrderBy(_ => random.Next()).ToArray();
        }

        public List<int[]> GerarPopulacaoInicial()
        {
            var populacao = new List<int[]>();
            for (int i = 0; i < TamanhoPopulacao; i++)
            {
                populacao.Add(GerarTabuleiroAleatorio());
            }
            return populacao;
        }

        public int[] SelecionarIndividuo(List<int[]> populacao)
        {
            var individuo1 = populacao[random.Next(populacao.Count)];
            var individuo2 = populacao[random.Next(populacao.Count)];

            return CalcularConflitos(individuo1) < CalcularConflitos(individuo2) ? individuo1 : individuo2;
        }

        // Realiza o cruzamento entre dois indivíduos
        public int[] Cruzamento(int[] pai1, int[] pai2)
        {
            int pontoCorte = random.Next(1, TamanhoTabuleiro - 1);
            var filho = pai1.Take(pontoCorte).Concat(pai2.Skip(pontoCorte)).ToArray();

            // Garante que o filho gerado é válido, caso contrário, gera um novo aleatório
            return filho.Distinct().Count() == TamanhoTabuleiro ? filho : GerarTabuleiroAleatorio();
        }

        public void Mutacao(int[] tabuleiro)
        {
            if (random.NextDouble() < TaxaMutacao)
            {
                int coluna = random.Next(TamanhoTabuleiro);
                int novaLinha = random.Next(TamanhoTabuleiro);
                tabuleiro[coluna] = novaLinha;
            }
        }

        public void Resolver()
        {
            var populacao = GerarPopulacaoInicial();

            for (int geracao = 0; geracao < MaxGeracoes; geracao++)
            {
                // Avalia a população e ordena pelo número de conflitos
                populacao = populacao.OrderBy(CalcularConflitos).ToList();

                // Verifica se encontrou uma solução
                if (CalcularConflitos(populacao.First()) == 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Solução encontrada na geração {geracao + 1}:");
                    ExibirTabuleiro(populacao.First());
                    return;
                }

                // Gera nova população
                var novaPopulacao = new List<int[]>();
                while (novaPopulacao.Count < TamanhoPopulacao)
                {
                    var pai1 = SelecionarIndividuo(populacao);
                    var pai2 = SelecionarIndividuo(populacao);

                    var filho = Cruzamento(pai1, pai2);
                    Mutacao(filho);

                    novaPopulacao.Add(filho);
                }

                populacao = novaPopulacao;
            }

            Console.WriteLine("Solução não encontrada após o número máximo de gerações.");
        }

        public void ExibirTabuleiro(int[] tabuleiro)
        {
            for (int i = 0; i < TamanhoTabuleiro; i++)
            {
                for (int j = 0; j < TamanhoTabuleiro; j++)
                {
                    Console.Write(tabuleiro[i] == j ? " R " : " . ");
                }
                Console.WriteLine();
            }
        }
    }
}
