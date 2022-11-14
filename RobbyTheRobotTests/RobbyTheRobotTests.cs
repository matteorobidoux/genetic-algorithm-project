using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyTheRobot;
using System;
using GeneticAlgorithm;
using System.IO;

namespace RobbyTheRobotTests
{
    [TestClass]
    public class RobbyTheRobotTests
    {
        //Tests constructor
        [TestMethod]
        public void TestCtor()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RobbyTheRobot.RobbyTheRobot(9, 1, 20, 100, 0.5, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RobbyTheRobot.RobbyTheRobot(200, 0, 20, 100, 0.5, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RobbyTheRobot.RobbyTheRobot(200, 1, 9, 100, 0.5, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RobbyTheRobot.RobbyTheRobot(200, 1, 20, 0, 0.5, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RobbyTheRobot.RobbyTheRobot(200, 1, 20, 100, 1.1, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RobbyTheRobot.RobbyTheRobot(200, 1, 20, 100, 0.5, -1, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RobbyTheRobot.RobbyTheRobot(200, 1, 20, 100, 0.5, 0.5, -1, null));
            var robby = Robby.CreateRobby(200, 1, 10, 100, 0.5, 0.5, 200, null);
            Assert.AreEqual(200, robby.NumberOfActions);
            Assert.AreEqual(1, robby.NumberOfTestGrids);
            Assert.AreEqual(10, robby.GridSize);
            Assert.AreEqual(100, robby.NumberOfGenerations);
            Assert.AreEqual(0.5, robby.MutationRate);
            Assert.AreEqual(0.5, robby.EliteRate);
        }

        //Determines if generated test grid with even dimensions gives proper can/empty ratio
        [TestMethod]
        public void TestGenerateRandomTestGridFiftyFiftyEvenGrid()
        {
            int? seed = null;

            var robby = Robby.CreateRobby(200, 1, 20, 100, 0.5, 0.5, 200, seed);
            var grid = robby.GenerateRandomTestGrid();
            
            int numOfCans = CountInGrid(grid, ContentsOfGrid.Can);
            int numOfEmpties = CountInGrid(grid, ContentsOfGrid.Empty);
            
            //Expects 50% for cans (rounded down if odd gridsize)
            int expectedCans = (int) Math.Floor(robby.GridSize * robby.GridSize * 0.5);

            //50$ empty from the gridsize (rounds up if odd gridsize)
            int expectedEmpty = (int) Math.Round(robby.GridSize * robby.GridSize * 0.5, MidpointRounding.AwayFromZero);

            Assert.AreEqual(expectedCans, numOfCans);
            Assert.AreEqual(expectedEmpty, numOfEmpties);
        }

        //Determines if generated test grid with odd dimensions gives proper can/empty ratio
        [TestMethod]
        public void TestGenerateRandomTestGridFiftyFiftyOddGrid()
        {
            int? seed = null;

            var robby = Robby.CreateRobby(200, 1, 21, 100, 0.5, 0.5, 200, seed);
            var grid = robby.GenerateRandomTestGrid();
            
            int numOfCans = CountInGrid(grid, ContentsOfGrid.Can);
            int numOfEmpties = CountInGrid(grid, ContentsOfGrid.Empty);
            
            //Expects 50% for cans (rounded down if odd gridsize)
            int expectedCans = (int) Math.Floor(robby.GridSize * robby.GridSize * 0.5);

            //50$ empty from the gridsize (rounds up if odd gridsize)
            int expectedEmpty = (int) Math.Round(robby.GridSize * robby.GridSize * 0.5, MidpointRounding.AwayFromZero);

            Assert.AreEqual(expectedCans, numOfCans);
            Assert.AreEqual(expectedEmpty, numOfEmpties);
        }

        //Expects Robby's fitness for one chromosome on 3 test grids to be -3000. (seeded)
        [TestMethod]
        public void TestComputeFitnessSeeded()
        {
            int? seed = 1;

            //create SEEDED robby + gene array
            var robby = Robby.CreateRobby(200, 3, 10, 100, 0.5, 0.5, 200, seed);
            int[] moves = MockGenerateGeneArray(seed);
            double score = MockComputeFitness(moves, robby, seed);
            
            Assert.AreEqual(-1000, score);
        }

        //Expects files written to disk to output consistent results. (seeded)
        [TestMethod]
        public void TestGeneratePossibleSolutionsSeeded()
        {
            int? seed = 1;

            var robby = Robby.CreateRobby(200, 1, 10, 100, 0.5, 0.5, 200, seed);

            //automatically create test directory
            string testOutputDirectory = "..\\..\\..\\GenerationsBinSeededTests";
            Directory.CreateDirectory(testOutputDirectory);

            //write file in ./TestRobbyTheRobot/GenerationBinTests/
            robby.GeneratePossibleSolutions(testOutputDirectory);

            string expectedOutputForGen1 = "0;200;103543260401264414446012515256354065603316423443123124216063400102326651013625603203222045550061632002323201135105626454634100002106356054602151044103114414422316602542563255255302260566264233226224516221063513511011610335433662602045203640234";
            string expectedOutputForGen20 = "10;200;524643255401363434440613532450603105603026422043142624215063632202331631064022411500025055500111642002020451115125341160634140001106636024152151044205134214622356502532623253155262220366364333420624514421063553411011216366431553605041203640262";
            string expectedOutputForGen100 = "10;200;524643255401363434440613532450603105603026422043142624215063632202331631064022411500025055500111642002020451115125341160634140001106636024152151044205134214622356502532623253155262220366364333420624514421063553411011216366431553605041203640262";

            Assert.AreEqual(expectedOutputForGen1, File.ReadAllText($"{testOutputDirectory}\\generation1.txt"));
            Assert.AreEqual(expectedOutputForGen20, File.ReadAllText($"{testOutputDirectory}\\generation20.txt"));
            Assert.AreEqual(expectedOutputForGen100, File.ReadAllText($"{testOutputDirectory}\\generation100.txt"));
        }

        //Expects the event to be raised 6 times for 100 generations. (not seeded)
        [TestMethod]
        public void TestGeneratePossibleSolutionsDelegateEvent()
        {
            int? seed = null;

            var robby = Robby.CreateRobby(200, 1, 10, 100, 0.5, 0.5, 200, seed);
            int eventCallCounter = 0;
            robby.FileWrittenEvent += delegate {
                eventCallCounter++;
            };

            //automatically create test directory
            string testOutputDirectory = "..\\..\\..\\GenerationsBinEventTests";
            Directory.CreateDirectory(testOutputDirectory);

            //write file in ./TestRobbyTheRobot/GenerationBinTests/
            robby.GeneratePossibleSolutions(testOutputDirectory);

            Assert.AreEqual(6, eventCallCounter);
        }

        //Mock functions

        private int CountInGrid(ContentsOfGrid[,] grid, ContentsOfGrid obj)
        {
            int counter = 0;

            foreach(var item in grid)
            {
                if(item == obj)
                {
                    counter++;
                }
            }

            return counter;
        }

        /// <summary> 
        /// Mocks an IChromosome int[] genes by generating a random or non-random
        /// int[243].
        /// <param name="seed">Potential random seed</returns>
        /// <returns>int[243] of genes</returns>
        private int[] MockGenerateGeneArray(int? seed)
        {
            //generate random SEEDED list of 243 moves
            int[] moves = new int[243];
            Random rand = GenerateRandom(seed);
            for (int i = 0; i < moves.Length; i++)
            {
                moves[i] = rand.Next(0, 7);
            }

            return moves;
        }

        /// <summary>
        /// Testing function of ComputeFitness() that only takes a random int[] of genes.
        /// IChromosome not required.
        /// </summary>
        /// <param name="moves">Gene array of moves</param>
        /// <param name="robby">Instance of Robby</param>
        /// <param name="seed">Potential random seed</returns>
        /// <returns>Fitness score for the given chromosome</returns>
        private double MockComputeFitness(int[] moves, IRobbyTheRobot robby, int? seed)
        {
            Random rand = GenerateRandom(seed);

            ContentsOfGrid[,] testGrid = robby.GenerateRandomTestGrid();
            double score = 0;
            int posX = 0;
            int posY = 0;

            for(int move = 0; move < robby.NumberOfActions; move++)
            {
                score += RobbyHelper.ScoreForAllele(moves, testGrid, rand, ref posX, ref posY);
            }

            return score;
        }

         /// <summary>
        /// This function simply returns an instance of a Random object
        /// given a seed or not.
        /// </summary>
        /// <param name="seed">Potential random seed</returns>
        /// <returns>Random object</returns>
        private Random GenerateRandom(int? seed) 
        {
            Random rand;

            if(seed != null)
            {
                int s = (int) seed;
                rand = new Random(s);
            }

            else
            {
                rand = new Random();
            }

            return rand;
        }
    }
}
