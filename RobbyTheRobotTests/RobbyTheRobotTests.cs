using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyTheRobot;
using System;
using System.IO;
using System.Threading;

namespace RobbyTheRobotTests
{
    [TestClass]
    public class RobbyTheRobotTests
    {
        //Tests constructor
        [TestMethod]
        public void TestCtor()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Robby.CreateRobby(9, 1, 20, 100, 0.5, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Robby.CreateRobby(200, 0, 20, 100, 0.5, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Robby.CreateRobby(200, 1, 9, 100, 0.5, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Robby.CreateRobby(200, 1, 20, 0, 0.5, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Robby.CreateRobby(200, 1, 20, 100, 1.1, 0.5, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Robby.CreateRobby(200, 1, 20, 100, 0.5, -1, 200, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Robby.CreateRobby(200, 1, 20, 100, 0.5, 0.5, -1, null));
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

        //Expects files written to disk to output consistent results. (seeded)
        [TestMethod]
        public void TestGeneratePossibleSolutionsSeeded()
        {
            int? seed = 1;

            var robby = Robby.CreateRobby(200, 1, 10, 10, 0.5, 0.5, 200, seed);

            //automatically create test directory
            string testOutputDirectory = "../../../TEMP/GenerationsBinSeededTests";
            Directory.CreateDirectory(testOutputDirectory);

            //write file in ./TestRobbyTheRobot/GenerationBinTests/
            robby.GeneratePossibleSolutions(testOutputDirectory);

            string expectedOutputForGen1 = "1,-970,200,1,0,3,5,4,3,2,6,0,4,0,1,2,6,4,4,1,4,4,4,6,0,1,2,5,1,5,2,5,6,3,5,4,0,6,5,6,0,3,3,1,6,4,2,3,4,4,3,1,2,3,1,2,4,2,1,6,0,6,3,4,0,0,1,0,2,3,2,6,6,5,1,0,1,3,6,2,5,6,0,3,2,0,3,2,2,2,0,4,5,5,5,0,0,6,1,6,3,2,0,0,2,3,2,3,2,0,1,1,3,5,1,0,5,6,2,6,4,5,4,6,3,4,1,0,0,0,0,2,1,0,6,3,5,6,0,5,4,6,0,2,1,5,1,0,4,4,1,0,3,1,1,4,4,1,4,4,2,2,3,1,6,6,0,2,5,4,2,5,6,3,2,5,5,2,5,5,3,0,2,2,6,0,5,6,6,2,6,4,2,3,3,2,2,6,2,2,4,5,1,6,2,2,1,0,6,3,5,1,3,5,1,1,0,1,1,6,1,0,3,3,5,4,3,3,6,6,2,6,0,2,0,4,5,2,0,3,6,4,0,2,3,4";
            string expectedOutputForAllGens = "10,200,0,0,3,1,4,4,2,1,5,4,0,6,2,6,4,4,1,2,1,6,0,3,1,0,5,1,0,2,5,6,3,2,3,0,5,5,6,0,3,3,0,0,5,5,3,5,4,0,1,2,3,1,2,4,5,2,6,0,2,5,4,0,0,1,0,6,0,5,6,1,0,1,3,1,2,6,2,5,4,2,3,2,4,3,6,2,2,3,1,4,2,3,2,3,6,1,6,5,1,0,0,6,5,3,6,2,6,1,2,1,3,1,0,5,6,2,2,4,5,0,6,3,0,4,5,0,0,0,6,1,0,4,3,5,6,0,5,4,1,0,1,1,2,1,0,4,4,4,2,3,1,1,4,4,1,1,4,6,2,2,1,6,6,4,2,5,3,2,5,2,3,6,4,5,2,4,5,3,0,4,5,6,4,5,6,6,2,4,1,2,6,3,0,2,3,2,2,0,5,1,3,3,3,4,3,6,4,4,1,3,2,0,1,2,1,1,4,1,0,6,3,3,0,0,6,3,1,2,4,0,2,0,4,5,2,0,3,6,4,6,2,3,5";

            Assert.AreEqual(expectedOutputForGen1, File.ReadAllText($"{testOutputDirectory}/generation1.txt"));
            Assert.AreEqual("2,"+expectedOutputForAllGens, File.ReadAllText($"{testOutputDirectory}/generation2.txt"));
            Assert.AreEqual("5,"+expectedOutputForAllGens, File.ReadAllText($"{testOutputDirectory}/generation5.txt"));
            Assert.AreEqual("10,"+expectedOutputForAllGens, File.ReadAllText($"{testOutputDirectory}/generation10.txt"));
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
            string testOutputDirectory = "../../../TEMP/GenerationsBinEventTests";
            Directory.CreateDirectory(testOutputDirectory);

            //write file in ./TestRobbyTheRobot/GenerationBinTests/
            robby.GeneratePossibleSolutions(testOutputDirectory);
            Thread.Sleep(500);

            Assert.AreEqual(6, eventCallCounter);
        }

        //Tests an invalid path when trying to GeneratePossibleSolutions()
        [TestMethod]
        public void TestInvalidPathForGeneratingSolutions()
        {
            var robby = Robby.CreateRobby(200, 1, 10, 100, 0.5, 0.5, 200);
            Assert.ThrowsException<ApplicationException>(() => robby.GeneratePossibleSolutions("../../../Blah"));
        }

        //helper functions
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
