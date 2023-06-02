using System;
using System.Collections.Generic;
using System.Text;

namespace NSGA2
{
    public abstract class GenericIndividual<GENE> where GENE : IComparable
    {
        public List<GENE> genes = new List<GENE>();

        // objectives to optimize
        public List<Objective> objectives = new List<Objective>(); // multi objective 가능

        // for calculating rank and domination
        public int rank; // 몇 번째 front에 있는가 (0에 가까울 수록 최적화 된 것이다.)
        public int dominationCount; // 나를 dominate하는 individual 수
        public List<GenericIndividual<GENE>> dominated; // 내가 dominate하는 다른 individual 목록

        // utility for random
        protected static Random random = new Random();
        
        // 유사한 smaple이 많으면 값이 작고, 고유할 수록 값이 크다.
        public double crowdingDistance;

        public GenericIndividual(int size)
        {
            /* size: gene 개수
             */ 
            for (int i = 0; i < size; i++) {genes.Add(CreateRandomGene());}
        }

        public void ResetDominationInfor() // 기존 domination관련 정보 초기화
        {
            dominationCount = 0;
            dominated = new List<GenericIndividual<GENE>>();
        }
        public virtual void Mutate(double mutation_probability)
        {
            /* 각 gene들을 mutation_probability확률로 변경
             */
            for (int i = 0; i < genes.Count; i++)
            {
                if (random.NextDouble() < mutation_probability)
                    genes[i] = CreateRandomGene();
            }
        }
        public abstract GENE CreateRandomGene();

        public virtual GenericIndividual<GENE> CrossOver(GenericIndividual<GENE> other, double probabilityCrossover)
        {
            /* probabilityCrossover 확률로 crossOver 연산 수행
             * 그 외의 경우 나 또는 other를 랜덤 반환
             */
            if (probabilityCrossover < random.NextDouble()) // 임계 확률을 넘지 않으면 부모 중 하나 랜덤 반환
                return random.NextDouble() < 0.5 ? this : other;

            GenericIndividual<GENE> child = CreateObject();
            for (int i = 0; i < child.genes.Count; i++)
            {
                child.genes[i] = (random.NextDouble() < 0.5 ? this : other).genes[i]; // 랜덤 확률로 gene 채택
            }
            // @@@@@@@@ 일반적인 cross over랑은 좀 다른 방식이네??
            return child;
        }
        public abstract GenericIndividual<GENE> CreateObject();

        public bool Dominates(GenericIndividual<GENE> other)
        {
            /* 모든 objective에 대해 other보다 뛰어난가?
             * 호출 전에 fitness를 평가하고 objecitves를 세팅해줘야 함
             */
            for (int i = 0; i < objectives.Count; i++)
            {
                if (!objectives[i].Dominates(other.objectives[i]))
                {
                    return false;
                }
            }
            return true;
        }
       
        public static Individual Breed(Individual parent1, Individual parent2)
        {
            /* 두 부모로 부터 자식 individual을 생성
             * Selection은 이미 된 상태이고
             * CrossOver, Mutation을 수행하여 자식 생성
             */
            Individual child = parent1.CrossOver(parent2);
            child.Mutate();
            return child;
        }

        public string GetPrintGenes()
        {
            string s = "(";
            for (int i = 0; i < genes.Count; i++)
            {
                s += $"{genes[i]}, ";
            }
            return s + ")";
        }
    }
}
