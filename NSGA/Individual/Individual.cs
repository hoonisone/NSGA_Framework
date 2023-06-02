using System;
using System.Collections.Generic;

namespace NSGA2
{
    public class Individual : GenericIndividual<int>
    {
        private static int SIZE = 10;
        private static int ACTION_NUM = 2;
        private static double MUTATION_PROBABILITY = 0.05;
        private static double CORSSOVER_PROBABILITY = 0.95;

        new public int dominationCount;
        new public List<Individual> dominated;

        new public void ResetDominationInfor()
        {
            dominationCount = 0;
            dominated = new List<Individual>();
        }

        public Individual() : base(SIZE) { }

        public override GenericIndividual<int> CreateObject()
        {
            return new Individual();
        }

        public override int CreateRandomGene()
        {
            return random.Next(ACTION_NUM);
        }

        public void Mutate(){
            Mutate(MUTATION_PROBABILITY);
        }
        public Individual CrossOver(Individual other){return (Individual)CrossOver(other, CORSSOVER_PROBABILITY);}

        public string GetPrint(Dictionary<string, bool> printFlags)
        {
            string s = "";
            if (!printFlags.ContainsKey("rank") || printFlags["rank"])
            {
                s += $"[Rank]: {this.rank}\n";
            }
            if (!printFlags.ContainsKey("crowdingDistance") || printFlags["crowdingDistance"])
            {
                s += $"[CrowdingDistance]: {this.crowdingDistance}\n";
            }
            if (!printFlags.ContainsKey("objectives") || printFlags["objectives"])
            {
                s += $"[Objectives]: \n"+
                    $"{GetPrintObjectives()}";
            }
            if (!printFlags.ContainsKey("genes") || printFlags["genes"])
            {
                s += $"[Genes]: {GetPrintGenes()}";
            }
            return s;
        }

        public string GetPrintObjectives()
        {
            string s = "";
            for (int i = 0; i < objectives.Count; i++)
            {
                s += $"({ i + 1}): {objectives[i].GetPrint()}\n";
            }
            return s;
        }
    }

}
