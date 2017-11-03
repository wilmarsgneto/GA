using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    class Principal
    {
        static void Main(string[] args)
        {
            int numberOfGenerations = 50;
            string[,] results = new string[numberOfGenerations, 7];



            for (int i = 0; i < numberOfGenerations; i++)
            {
                Console.Clear();
                Console.Write($"Number of Executions {i + 1}/{numberOfGenerations}");

                GA ga = new GA();

                ga.firstPop();
                ga.population = ga.population.OrderByDescending(x => x.fitness).ToList();

                int geracoesComResultadoIgual = 0;
                double melhorFitness = 0;

                for (int j = 0; j < ga.NUMBER_GENERATIONS; j++)
                {
                    melhorFitness = ga.population.First().fitness;
           
                    ga.Crossover();
                    ga.Mutation();
                    ga.Elitism();
                    ga.NextGenerationSelection();

                    // Tratamento para retirar convergencia local
                    if (melhorFitness == ga.population.First().fitness)
                        geracoesComResultadoIgual++;
                    else
                        geracoesComResultadoIgual = 0;

                    if (geracoesComResultadoIgual == 6)
                    {
                        ga.firstPop();
                        geracoesComResultadoIgual = 0;
                    }

                }


                results[i, 0] = ga.POP_SIZE.ToString();
                results[i, 1] = ga.NUMBER_GENERATIONS.ToString();
                results[i, 2] = (100 * ga.MUTATION_RATE).ToString();
                results[i, 3] = (100 * ga.REPRODUCTION_RATE).ToString();
                results[i, 4] = ga.population.First().x.ToString("0.0000");
                results[i, 5] = ga.population.First().y.ToString("0.0000");
                results[i, 6] = ga.population.First().fitness.ToString("0.0000");
            }


            //Dá saída em um arquivo
            var nomeArquivo = $"Results GA{DateTime.Now.ToString("yyyyMMdd-HHmmffff")}.txt";
            var fileDirectory = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + nomeArquivo;

            if (!System.IO.File.Exists(fileDirectory))
                using (System.IO.File.Create(fileDirectory)) { }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileDirectory, true))
            {
                file.WriteLine("-------------------------- Results --------------------------\n\n");

                file.WriteLine(String.Format("Fitness|Execução|POP Size|Number of Generations|Mutation Rate|Crossover Rate|X      |Y"));
                for (int i = 0; i < numberOfGenerations; i++)
                    file.WriteLine(String.Format("{7}|{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                        (i + 1).ToString().PadRight(8),
                        results[i, 0].PadRight(20),
                        results[i, 1].PadRight(18),
                        (results[i, 2] + "%").PadRight(15),
                        (results[i, 3] + "%").PadRight(17),
                        results[i, 4].PadLeft(7),
                        results[i, 5].PadLeft(7),
                        results[i, 6].PadLeft(7)));

                file.Write("\n\n------------------------------------------------------------------------");


            }

            System.Diagnostics.Process.Start(fileDirectory);
            Console.ReadKey();
            System.IO.File.Delete(fileDirectory);

        }
    }
}
