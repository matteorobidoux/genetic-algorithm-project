using System;
using System.Collections.Generic;
using GeneticAlgorithm;

namespace RobbyTheRobot
{
    internal class RobbyTheRobot : IRobbyTheRobot
    {
        private int _numberOfGenerations;
        private int _populationSize;
        private int _numberOfTrials; // The number of times the fitness function should be called when computing the result
        private int? _potentialSeed;
        private int _numberOfActions;
        private int _numberOfTestGrids;
        private double _mutationRate;
        private double _eliteRate;
        private int _gridSize;
        public int NumberOfActions => throw new NotImplementedException(); //steps for robby
        public int NumberOfTestGrids {get => _numberOfTestGrids;} //decide myself
        public int GridSize {get => _gridSize;} //constant 10
        public int NumberOfGenerations {get => _numberOfGenerations;} //set in constructor, by user
        public double MutationRate {get => _mutationRate;} //set in constructor, by user 
        public double EliteRate {get => _eliteRate;} //set in constructor, by user

        public RobbyTheRobot(int gridSize, int numberOfTestGrids, int mutationRate, int eliteRate, int numberOfGenerations, int populationSize, int numberOfTrials, int? potentialSeed = null)
        {
            _gridSize = gridSize;
            _numberOfTestGrids = numberOfTestGrids;
            _mutationRate = mutationRate;
            _eliteRate = eliteRate;
            _numberOfGenerations = numberOfGenerations;
            _populationSize = populationSize;
            _numberOfTrials = numberOfTrials;

            if(potentialSeed != null)
            {
                _potentialSeed = potentialSeed;
            }
        }

        public void GeneratePossibleSolutions(string folderPath)
        {
            throw new NotImplementedException();
        }

        public ContentsOfGrid[,] GenerateRandomTestGrid()
        {
            Random rand = GenerateRandom();

            ContentsOfGrid[,] grid = new ContentsOfGrid[GridSize, GridSize];

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
            int expectedFiftyPercent = (int) Math.Floor(GridSize * GridSize * 0.5);
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

        /// <summary> TO-DO
        /// This function computes the fitness of a chromosome for a given random 
        /// of TestGrid. It then computes the score and returns a double.
        /// </summary>
        /// <param name="moves">IChoromosome, containing gene list of 243 moves</param>
        /// <returns>Fitness score for the given chromosome</returns>
        public double ComputeFitness(IChromosome chromosome, IGeneration generation)
        {
            Random rand = GenerateRandom();

            ContentsOfGrid[,] testGrid;
            int posX = 0;
            int posY = 0;

            testGrid = GenerateRandomTestGrid();
            double score = RobbyHelper.ScoreForAllele(chromosome.Genes, testGrid, rand, ref posX, ref posY);

            return score;
        }

        /// <summary>
        /// Testing function of ComputeFitness() that only takes a random gene.
        /// </summary>
        /// <param name="moves">Gene array of moves</param>
        /// <returns>Fitness score for the given gene</returns>
        public double ComputeFitness(int[] moves)
        {
            Random rand = GenerateRandom();

            ContentsOfGrid[,] testGrid;
            int posX = 0;
            int posY = 0;

            testGrid = GenerateRandomTestGrid();
            double score = RobbyHelper.ScoreForAllele(moves, testGrid, rand, ref posX, ref posY);

            return score;
        }

        /// <summary>
        /// This function simply returns an instance of a Random object
        /// given a seed or not.
        /// </summary>
        /// <returns>Random object</returns>
        private Random GenerateRandom() 
        {
            Random rand;

            if(_potentialSeed != null)
            {
                int seed = (int) _potentialSeed;
                rand = new Random(seed);
            }

            else
            {
                rand = new Random();
            }

            return rand;
        }
    }
}
