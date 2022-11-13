using System;

namespace RobbyTheRobot
{
    public static class Robby
    {
        public static IRobbyTheRobot CreateRobby(int numberOfActions,
                                                 int numberOfTestGrids,
                                                 int gridSize,
                                                 int numberOfGenerations,
                                                 double mutationRate,
                                                 double eliteRate,
                                                 int populationSize,
                                                 int numberOfTrials,
                                                 int? potentialSeed = null)
        {
            return new RobbyTheRobot(numberOfActions,
                                     numberOfTestGrids,
                                     gridSize,
                                     numberOfGenerations,
                                     mutationRate,
                                     eliteRate,
                                     populationSize,
                                     numberOfTrials,
                                     potentialSeed);
        }
    }
}