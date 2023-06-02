using System;
using System.Collections.Generic;
using System.Text;

namespace NSGA2
{
    public class EnvironmentFactory
    {
        public static Environment Get()
        {
            return new SampleEnvironment();
        }
    }
}
