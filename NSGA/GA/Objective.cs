using System;
using System.Collections.Generic;
using System.Text;

namespace NSGA2
{
    public enum ObjectiveType { Minimize, Maximize }
    public class Objective
    {
        public ObjectiveType type;
        public string name = "no_name";
        public float fitness;

        public Objective(ObjectiveType type, string name = "no_name", float fitness = 0)
        {
            this.type = type;
            this.name = name;
            this.fitness = fitness;
        }

        public string GetPrint()
        {
            return $"({type}) \"{name}\": {fitness}";
        }

        public bool Dominates(Objective other)
        {
            switch (type)
            {
                case ObjectiveType.Minimize:
                    return this.fitness < other.fitness;
                case ObjectiveType.Maximize:
                    return other.fitness < this.fitness;
            }
            throw new Exception("잘못된 type");
        }
    }
}
