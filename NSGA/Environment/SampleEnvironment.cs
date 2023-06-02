using System;
using System.Collections.Generic;
using System.Text;

namespace NSGA2
{
    class SampleEnvironment : Environment
    {
        override public List<Action> GetActionList(Individual individual)
        {
            /* individual의 genes를 ActionList로 매핑
             */
            List<Action> actions = new List<Action>();
            foreach (int gene in individual.genes)
            {
                actions.Add((Action)gene);
            }
            return actions;
        }

        override public List<Objective> GetObjectives(List<Action> actions)
        {
            float attackCount = 0;
            float runCount = 0;
            foreach (Action action in actions)
            {
                switch (action)
                {
                    case Action.Atack:
                        attackCount += 1;
                        break;
                    case Action.Run:
                        runCount += 1;
                        break;
                }
            }

            /* 아래처럼 하면 Maximize해야 할 objective가 총 2개 있는 것
             * 서로 다른 objective를 넘겨도 내부 알고리즘이 objective들을 모두 고려하여 최적의 해를 찾아준다.
             */
            List<Objective> objectives = new List<Objective>();
            objectives.Add(new Objective(ObjectiveType.Maximize, "attack score1", attackCount * runCount));
            objectives.Add(new Objective(ObjectiveType.Maximize, "attack score2", attackCount + runCount));


            /* 각각의 objective에 대한 individual의 fitness 값 */
            return objectives; // ex( boss HP, my HP, ...)
        }
    }
}
