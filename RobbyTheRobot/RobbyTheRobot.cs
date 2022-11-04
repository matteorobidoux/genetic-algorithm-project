using System;

namespace RobbyTheRobot
{
    internal class RobbyTheRobot : IRobbyTheRobot
    {
        private int _numberOfGenerations;
        private int _populationSize;
        private int _numberOfTrials;
        private int? _potentialSeed;
        private int _numberOfActions;
        private int _numberOfTestGrids;
        private double _mutationRate;
        private double _eliteRate;
        private int _gridSize;
        public int NumberOfActions => throw new NotImplementedException(); //steps for robby
        public int NumberOfTestGrids => throw new NotImplementedException(); //decide myself
        public int GridSize {get => _gridSize;} //constant 10
        public int NumberOfGenerations {get => _numberOfGenerations;} //set in constructor, by user
        public double MutationRate => throw new NotImplementedException(); //set in constructor, by user 
        public double EliteRate => throw new NotImplementedException(); //set in constructor, by user

        public RobbyTheRobot(int gridSize, int numberOfGenerations, int populationSize, int numberOfTrials, int? potentialSeed = null)
        {
            _gridSize = gridSize;
            _numberOfGenerations = numberOfGenerations;
            _populationSize = populationSize;
            _numberOfTrials = numberOfTrials;

            if(potentialSeed != null)
            {
                _potentialSeed = potentialSeed;
            }

            else
            {
                _potentialSeed = null;
            }
        }

        public void GeneratePossibleSolutions(string folderPath)
        {
            throw new NotImplementedException();
        }

        public ContentsOfGrid[,] GenerateRandomTestGrid()
        {
            ContentsOfGrid[,] grid = new ContentsOfGrid[GridSize, GridSize];
            Random rand = new Random();

            //Instantiate grid with empty 
            for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        grid[x, y] = ContentsOfGrid.Empty;
                    }
                }

            //Replace empty with cans randomly
            int cans = 0;
            int expectedFiftyPercent = (int) Math.Round(GridSize * GridSize * 0.5);
            do
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if(cans == expectedFiftyPercent)
                    {
                        break;
                    }

                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        if(cans == expectedFiftyPercent)
                        {
                            break;
                        }

                        int randCont = rand.Next(0, 2);
                        ContentsOfGrid cont = (ContentsOfGrid) randCont;

                        if(cont == ContentsOfGrid.Can && grid[x,y] == ContentsOfGrid.Empty && cans < expectedFiftyPercent)
                        {
                            grid[x,y] = cont;
                            cans++;
                        }

                        else 
                        {
                            continue;
                        }
                    }
                }

            } while (cans < expectedFiftyPercent);

            return grid;
        }
    }
}
