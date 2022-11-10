using System;

namespace RobbyTheRobot
{
    public static class Robby
    {
        public static IRobbyTheRobot CreateRobby(int gridSize, int numberOfTestGrids, int mutationRate, int eliteRate, int numberOfGenerations, int populationSize, int numberOfTrials, int? potentialSeed = null)
        {
            //ESSENTIAL: numberOfGenerations, populationSize, numberOfTrials, potentialSeed
            return new RobbyTheRobot(gridSize, numberOfTestGrids, numberOfTestGrids, mutationRate, eliteRate, populationSize, numberOfTrials, potentialSeed);
        }
    }
}