using System;

namespace RobbyTheRobot
{
    public static class Robby
    {
        public static IRobbyTheRobot CreateRobby(int numberOfActions,
                                                 int numberOfTrials,
                                                 int numberOfGenerations,
                                                 double mutationRate,
                                                 double eliteRate,
                                                 int populationSize,
                                                 int? potentialSeed = null)
        {
            if(numberOfActions < 10)
            {
                throw new ArgumentOutOfRangeException($"Number Of Actions minimun is 10. Got {numberOfActions}");
            }

            if(numberOfTrials < 1)
            {
                throw new ArgumentOutOfRangeException($"Number of test grids minimum is 1. Got {numberOfTrials}");
            }

            if(numberOfGenerations < 1)
            {
                throw new ArgumentOutOfRangeException($"Number of generations minimum is 1. Got {numberOfGenerations}");
            }

            if(mutationRate < 0 || mutationRate > 1)
            {
                throw new ArgumentOutOfRangeException($"Mutation rate expected between 0 and 1. Got {mutationRate}");
            }

            if(eliteRate < 0 || eliteRate > 1)
            {
                throw new ArgumentOutOfRangeException($"Elite rate expected between 0 and 1. Got {eliteRate}");
            }

            if(populationSize < 10)
            {
                throw new ArgumentOutOfRangeException($"Population size minimum is 10. Got {populationSize}");
            }
            
            return new RobbyTheRobot(numberOfActions,
                                     numberOfTrials,
                                     numberOfGenerations,
                                     mutationRate,
                                     eliteRate,
                                     populationSize,
                                     potentialSeed);
        }
    }
}