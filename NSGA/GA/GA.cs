using System;
using System.Linq;
using System.Collections.Generic;



namespace NSGA2
{
    public class GA
    {
        
        static Random random = new Random();

        private int populationSize;
        public List<Individual> population = new List<Individual>();
        public List<List<Individual>> fronts = new List<List<Individual>>(); // rank 별로 front들을 저장
        public List<Individual> nonDominatedFront = new List<Individual>();

        public GA(int populationSize)
        {
            this.populationSize = populationSize;
            for (int i = 0; i < populationSize; i++)
            {
                Individual individual = new Individual();
                population.Add(individual);    
            }
        }

        public void NextGeneration()
        {
            population = Offspring();
        }

        // Offspring: Produces the entire new generation of offspring 
        public List<Individual> Offspring()
        {
            List<Individual> offspring = new List<Individual>(populationSize);

            for (int i = 0; i < populationSize; i++)
            {
                offspring.Add(Individual.Breed(SelectParent(), SelectParent()));
            }
            return offspring;
        }

        // SelectParent: Selects an individual from the population to reproduce, while giving priority to individuals in the beginning (= better ranked) of the list
        private Individual SelectParent()
        {
            int chosen = (int)Math.Floor(((double)population.Count - 1e-3) * Math.Pow((random.NextDouble()), 2));
            return population[chosen];
        }

        public void FastNonDominatedSort()
        {
            nonDominatedFront.Clear();
            fronts.Clear();

            for (int i = 0; i < population.Count; i++)
            {
                Individual p = population[i];

                p.ResetDominationInfor();

                for (int j = 0; j < population.Count; j++)
                {
                    if (i == j) continue;
                    Individual q = population[j];

                    if (p.Dominates(q))
                        p.dominated.Add(q);
                    else if (q.Dominates(p))
                        p.dominationCount++;
                }

                if (p.dominationCount == 0)
                {
                    p.rank = 0;
                    nonDominatedFront.Add(p);
                }
            }

            fronts.Add(nonDominatedFront);

            int currentFront = 0;

            while (currentFront < fronts.Count)
            {
                List<Individual> nextFront = new List<Individual>();

                foreach (var p in fronts[currentFront])
                {
                    foreach (var q in p.dominated)
                    {
                        q.dominationCount--;

                        if (q.dominationCount == 0)
                        {
                            q.rank = currentFront + 1;
                            nextFront.Add(q);
                        }
                    }
                }

                currentFront++;
                if (nextFront.Count != 0)
                    fronts.Add(nextFront);
            }

            population = fronts.SelectMany(i => i).ToList(); // rank 순으로 individual들을 정렬
        }


        // CrowdingDistance: Calculates Crowding Distance for each individual of a given front
        public void CrowdingDistance(List<Individual> front)
        {
            int size = front.Count;
            int numObjectives = front[0].objectives.Count;
            front.ForEach(p => p.crowdingDistance = 0);

            for (int m = 0; m < numObjectives; m++)
            {
                front = front.OrderBy(x => x.objectives[m].fitness).ToList();

                front[0].crowdingDistance = double.PositiveInfinity;
                front[size - 1].crowdingDistance = double.PositiveInfinity;

                for (int i = 1; i < size - 1; i++)
                {
                    double max = front.Max(p => p.objectives[m].fitness);
                    double min = front.Min(p => p.objectives[m].fitness);
                    front[i].crowdingDistance += (front[i + 1].objectives[m].fitness - front[i - 1].objectives[m].fitness) / (max - min);
                }
                break;
            }
        }

        public void Print(Dictionary<string, bool> printFlags)
        {
            Console.WriteLine(GetPrint(printFlags));
        }

        public string GetPrint(Dictionary<string, bool> printFlags)
        {
            string s = "";
            for (int i = 0; i < populationSize; i++)
            {
                s += $"************************G({i + 1})************************\n" +
                    $"{population[i].GetPrint(printFlags)}\n";
            }
            return s;
        }
    }
}
