using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithm;

namespace RobbyTheRobot
{
    internal class RobbyTheRobot : IRobbyTheRobot
    {
        public int NumberOfGenes {get => 243;} //length of int[] _genes in Chromosome
        public int LengthOfGene {get => 7;} //variety of genes (different actions robby can do), from 0 to 6.
        private int _numberOfActions; // number of moves robby can do (200)
        private int _numberOfTrials; //The number of times the fitness function should be called when computing the result
        private int _gridSize; //size of one dimension of the grid
        private int _numberOfGenerations; //number of generations
        private double _mutationRate; //between 0 and 1
        private double _eliteRate; //between 0 and 1
        private int _populationSize; //number of chromosomes initially (200)
        private int? _potentialSeed; //for making random predictable
        public int NumberOfActions {get => _numberOfActions;} //steps for robby
        public int NumberOfTestGrids {get => _numberOfTrials;} //decide myself
        public int GridSize {get => _gridSize;} //constant 10
        public int NumberOfGenerations {get => _numberOfGenerations;} //set in constructor, by user
        public double MutationRate {get => _mutationRate;} //set in constructor, by user 
        public double EliteRate {get => _eliteRate;} //set in constructor, by user
        internal RobbyTheRobot(int numberOfActions,
                               int numberOfTrials,
                               int gridSize,
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

            if(gridSize < 10)
            {
                throw new ArgumentOutOfRangeException($"Grid size minimum is 10. Got {gridSize}");
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

            _numberOfActions = numberOfActions;
            _numberOfTrials = numberOfTrials;
            _gridSize = gridSize;
            _numberOfGenerations = numberOfGenerations;
            _mutationRate = mutationRate;
            _eliteRate = eliteRate;
            _populationSize = populationSize;
            _potentialSeed = potentialSeed;
        }

        public event FileHandler FileWrittenEvent;

        public void GeneratePossibleSolutions(string folderPath)
        {
            if(!Directory.Exists(folderPath))
            {
                throw new ApplicationException("Folder path specified not found.");
            }

            var genAlg = GeneticLib.CreateGeneticAlgorithm(_populationSize, NumberOfGenes, LengthOfGene, _mutationRate, _eliteRate, NumberOfTestGrids, ComputeFitness, _potentialSeed);
            IGeneration currentGen;

            for(int generationNum = 1; generationNum <= NumberOfGenerations; generationNum++)
            {
                currentGen = genAlg.GenerateGeneration();
                
                if(generationNum == 1 ||
                   generationNum == Math.Round(NumberOfGenerations * 0.02) || 
                   generationNum == Math.Round(NumberOfGenerations * 0.1) || 
                   generationNum == Math.Round(NumberOfGenerations * 0.2) || 
                   generationNum == Math.Round(NumberOfGenerations * 0.5) || 
                   generationNum == NumberOfGenerations)
                {
                    IChromosome bestChromosome = currentGen[0]; //IGeneration sorted to have candidate with max fitness as index 0.
                    string candidate = currentGen.MaxFitness + "," + NumberOfActions + "," + string.Join(",", bestChromosome.Genes);
                    WriteToFile(folderPath + $"/generation{generationNum}.txt", candidate);

                    if(FileWrittenEvent != null)
                    {
                        string metadata = $"Generation #{generationNum} successfully written to disk.";
                        FileWrittenEvent(metadata);
                    }
                }
            }
        }

        /// <summary>
        /// Helper function to write files to disk provided a path and text.
        /// Runs in a separate Thread. If file exists, it is overriden.
        /// </summary>
        /// <param name="path">Path to where the file will be written</param>
        /// <param name="text">Input string to store in the File</param>
        private void WriteToFile(string path, string text)
        {
            Task.Run(() => File.WriteAllText(path, text, Encoding.UTF8));
        }

        public ContentsOfGrid[,] GenerateRandomTestGrid()
        {
            ContentsOfGrid[,] grid = new ContentsOfGrid[GridSize, GridSize];
            int expectedFiftyPercent = (int) Math.Floor(GridSize * GridSize * 0.5);
            HashSet<int> indexesForCans = GenerateRandomCanIndexes(expectedFiftyPercent);
            int counterIndexForCans = 0;

            for(int y = 0; y < grid.GetLength(0); y++)
            {
                for(int x = 0; x < grid.GetLength(1); x++)
                {
                    if(indexesForCans.Contains(counterIndexForCans))
                    {
                        grid[y,x] = ContentsOfGrid.Can;
                    }

                    else
                    {
                        grid[y,x] = ContentsOfGrid.Empty;
                    }

                    counterIndexForCans++;
                }
            }

            return grid;
        }

        /// <summary>
        /// Generates a random HashSet containing the indexes
        /// at which a can will placed in a random test grid.
        /// </summary>
        /// <param name="numOfIndexes">Specifies the number of random indexes to generate</param>
        private HashSet<int> GenerateRandomCanIndexes(int numOfIndexes)
        {
            Random rand = GenerateRandom();
            
            HashSet<int> indexes = new HashSet<int>(numOfIndexes);

            while(indexes.Count < numOfIndexes)
            {
                int add = rand.Next(0, numOfIndexes);
                indexes.Add(add);
            }

            return indexes;
        }

        /// <summary>
        /// This function computes the fitness of a chromosome for a given random 
        /// of TestGrid. It then computes the score for a given number of actions (moves) 
        /// that Robby can do on the grid and returns a double.
        /// </summary>
        /// <param name="moves">IChoromosome, containing gene list of 243 genes (moves)</param>
        /// <param name="generation">Currrent generation</param>
        /// <returns>Fitness score for the given chromosome</returns>
        private double ComputeFitness(IChromosome chromosome, IGeneration generation)
        {
            Random rand = GenerateRandom();

            ContentsOfGrid[,] testGrid = GenerateRandomTestGrid();
            double score = 0;
            int posX = rand.Next(0, GridSize);
            int posY = rand.Next(0, GridSize);

            for(int move = 0; move < NumberOfActions; move++)
            {
                score += RobbyHelper.ScoreForAllele(chromosome.Genes, testGrid, rand, ref posX, ref posY);
            }
        
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
