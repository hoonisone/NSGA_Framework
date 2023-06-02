using System.Collections.Generic;

namespace NSGA2
{
    public enum Action { Atack, Run }

    public abstract class Environment
    {
        /*각 object function이 최대화 되어야 하는지 최소화 되어야 하는지 명시*/

        public Environment()
        {

        }
        public void Evaluate(List<Individual> population)
        {
            /* population의 각 individual을 환경에 대해 평가하고 objectives 세팅
             */
            foreach (Individual individual in population)
            {
                individual.objectives = GetObjectives(GetActionList(individual));
            }
        }

        abstract public List<Action> GetActionList(Individual individual);
           


        abstract public List<Objective> GetObjectives(List<Action> actionList);
    }
}
