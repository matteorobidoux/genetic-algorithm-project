using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyTheRobot;
using System;
using GeneticAlgorithm;

namespace TestRobbyTheRobot
{
    [TestClass]
    public class TestRobbyTheRobot
    {
        [TestMethod]
        public void TestGenerateRandomTestGridFiftyFifty()
        {
            var robby = Robby.CreateRobby(200, 1, 10, 100, 404, 404, 200, 1, null);
            var grid = robby.GenerateRandomTestGrid();
            
            int numOfCans = 0;
            int numOfEmpties = 0;
            foreach (var item in grid)
            {
                if(item is ContentsOfGrid.Can)
                {
                    numOfCans++;
                }

                else
                {
                    numOfEmpties++;
                }
            }
            
            //Expects 50% for cans and empty from the gridsize
            int expected = (int) Math.Round(robby.GridSize * robby.GridSize * 0.5);

            Assert.AreEqual(expected, numOfCans);
            Assert.AreEqual(expected, numOfEmpties);
        }

        [TestMethod]
        public void TestComputeFitnessSeeded()
        {
            //create SEEDED robby
            var robby = Robby.CreateRobby(200, 1, 10, 100, 404, 404, 200, 1, 1);

            //generate random SEEDED list of 243 moves
            int[] moves = new int[243];
            Random rand = new Random(1);
            for (int i = 0; i < moves.Length; i++)
            {
                moves[i] = rand.Next(0, 7);
            }

            double score = robby.ComputeFitness(moves);
            
            Console.WriteLine("FITNESS: " + score);
            Assert.AreEqual(-1000, score);
        }
    }
}
