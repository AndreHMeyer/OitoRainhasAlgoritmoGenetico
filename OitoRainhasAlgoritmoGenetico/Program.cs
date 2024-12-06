namespace OitoRainhasAlgoritmoGenetico
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Digite o tamanho do tabuleiro (NxN): ");
                int tamanhoTabuleiro = int.Parse(Console.ReadLine());

                Console.WriteLine("Digite o tamanho da população: ");
                int tamanhoPopulacao = int.Parse(Console.ReadLine());

                Console.WriteLine("Digite o número máximo de gerações: ");
                int maxGeracoes = int.Parse(Console.ReadLine());

                Console.WriteLine("Digite a taxa de cruzamento (0.0 a 1.0): ");
                double taxaCruzamento = double.Parse(Console.ReadLine());

                Console.WriteLine("Digite a taxa de mutação (0.0 a 1.0): ");
                double taxaMutacao = double.Parse(Console.ReadLine());

                var nQueens = new OitoRainhas(tamanhoTabuleiro, tamanhoPopulacao, maxGeracoes, taxaCruzamento, taxaMutacao);
                nQueens.Resolver();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}
