using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    class GA
    {
        //parameters
        public int POP_SIZE = 100;
        public int CHROMOSSOME_SIZE = 44;
        public Random random = new Random();
        public int NUMBER_GENERATIONS = 100;
        public double MUTATION_RATE = 0.005;
        public double REPRODUCTION_RATE = 0.65;

        public List<individual> population = new List<individual>();
        public List<individual> offspring = new List<individual>();


        public void firstPop()
        {
            //first generation genereated randomly
            if (population.Count > 0)
            {
                population.RemoveRange(1, population.Count - 1);
            }

            for (int i = population.Count; i < POP_SIZE; i++)
            {
                int[] cromossomo = new int[CHROMOSSOME_SIZE];
                for (int j = 0; j < CHROMOSSOME_SIZE; j++)
                    cromossomo[j] = random.Next(2);
                population.Add(new individual(cromossomo));
            }
        }
        public void Crossover()
        {
            int numberOfCrossovers = (int)((POP_SIZE / 2) * REPRODUCTION_RATE) * 2; //always a pair number
            double FitnessSum = 0;


            foreach (individual x in population)
                FitnessSum += x.fitness;

            for (int i = 0; i < numberOfCrossovers; i += 2)
            {
                int P1 = RouletteSelect(FitnessSum);
                int P2 = RouletteSelect(FitnessSum);
                
                ExchangeGeneticMaterial(population[P1], population[P2], out individual F1, out individual F2);
                offspring.Add(F1);
                offspring.Add(F2);
            }
        }
        public void Mutation()
        {
            foreach (individual ind in offspring)
                for (int i = 0; i < CHROMOSSOME_SIZE; i++)
                {
                    double rand = random.NextDouble();
                    if (rand <= MUTATION_RATE)
                        ind.chromosome[i] = ind.chromosome[i] == 0 ? 1 : 0;
                }
        }

        private void ExchangeGeneticMaterial(individual P1, individual P2, out individual F1, out individual F2)
        {
            int chromossomesize = CHROMOSSOME_SIZE / 2; 
            int indexX = random.Next(chromossomesize); 
            int indexY = chromossomesize + random.Next(chromossomesize); 
            int[] chromosome_F1 = new int[CHROMOSSOME_SIZE];
            int[] chromosome_F2 = new int[CHROMOSSOME_SIZE];


            for (int i1 = 0, i2 = chromossomesize; i1 < chromossomesize; i1++, i2++)
            {
                chromosome_F1[i1] = (i1 < indexX) ? P1.chromosome[i1] : P2.chromosome[i1];
                chromosome_F2[i1] = (i1 < indexX) ? P2.chromosome[i1] : P1.chromosome[i1];

                chromosome_F1[i2] = (i2 < indexY) ? P1.chromosome[i2] : P2.chromosome[i2];
                chromosome_F2[i2] = (i2 < indexY) ? P2.chromosome[i2] : P1.chromosome[i2];
            }

            F1 = new individual(chromosome_F1);
            F2 = new individual(chromosome_F2);
        }
        private int RouletteSelect(double Fitnesssum)
        {
            int index = population.Count - 1;
            double rand = random.NextDouble() * Fitnesssum;
            double accumulated = 0;

            for (int i = 0; i < population.Count; i++)
            {
                accumulated += population[i].fitness;
                if (rand <= accumulated)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        public void Elitism() {
            population.OrderByDescending(rrs=> rrs.fitness).ToList();
            individual temp = population[0];
            population.Clear();
            population.Add(temp);
            firstPop();



        }
        public void NextGenerationSelection()
        {
            foreach (individual item in offspring)
                population.Add(item);

            population = population.OrderByDescending(x => x.fitness).ToList();

            for (int i = population.Count - 1; i >= POP_SIZE; i--)
                population.RemoveAt(i);

            offspring.Clear();
        }
    }
}
