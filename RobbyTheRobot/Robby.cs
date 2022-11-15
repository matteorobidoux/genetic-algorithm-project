using System;

namespace RobbyTheRobot
{
    public static class Robby
    {
        public static IRobbyTheRobot CreateRobby(int numberOfActions,
                                                 int numberOfTrials,
                                                 int gridSize,
                                                 int numberOfGenerations,
                                                 double mutationRate,
                                                 double eliteRate,
                                                 int populationSize,
                                                 int? potentialSeed = null)
        {
            return new RobbyTheRobot(numberOfActions,
                                     numberOfTrials,
                                     gridSize,
                                     numberOfGenerations,
                                     mutationRate,
                                     eliteRate,
                                     populationSize,
                                     potentialSeed);
        }
    }
}