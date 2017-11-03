using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA
{
    class individual : IComparable<individual>
    {

        public int chromosome_size;
        public int[] chromosome;
        public int MaxValue = 100;
        public int MinimumValue = -100;
        public double x;
        public double y;
        public double fitness;


      

        public individual(int[] cromossomo)
        {
            chromosome = cromossomo;
            chromosome_size = cromossomo.Length;
            decodeChromossome();
            fitness = Fitness();
        }
        public double Fitness()
        {
            var fitness = 0.5 - ((Math.Pow(Math.Sin(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2))), 2) - 0.5) / Math.Pow(1.0 + (0.001 * (Math.Pow(x, 2) + Math.Pow(y, 2))), 2));
            return fitness;
        }

        private void decodeChromossome()
        {
            int numberOfBits = (chromosome_size / 2);
            var range = MaxValue - MinimumValue;

            for (int i = numberOfBits - 1; i >= 0; i--)
            {
                x += chromosome[i] * Math.Pow(2, i);
                y += chromosome[i + numberOfBits] * Math.Pow(2, i);
            }

            x = (x * (range / Math.Pow(2, numberOfBits))) + MinimumValue;
            y = (y * (range / Math.Pow(2, numberOfBits))) + MinimumValue;
        }
        public int Compare(individual x, individual y)
        {
            var FitnessX = x.fitness;
            var FitnessY = y.fitness;
            if (FitnessX == FitnessY)
                return 0;
            return (FitnessX < FitnessY) ? -1 : 1;
        }
        public int CompareTo(individual individual2)
        {
            if (this.fitness == individual2.fitness)
                return 0;
            return (this.fitness < individual2.fitness) ? -1 : 1;
        }
        public override string ToString()
        {
            String str = String.Format("X: {0}\tY: {1}\tFitness: {2}", x, y, fitness);
            return str;
        }
    }
}
