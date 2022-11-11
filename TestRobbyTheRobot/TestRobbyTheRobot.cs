using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyTheRobot;
using System;
using GeneticAlgorithm;
using System.IO;

namespace TestRobbyTheRobot
{
    [TestClass]
    public class TestRobbyTheRobot
    {
        [TestMethod]
        public void TestGenerateRandomTestGridFiftyFifty()
        {
            var robby = Robby.CreateRobby(200, 1, 10, 100, 0.5, 0.5, 200, 1, null);
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
            var robby = Robby.CreateRobby(200, 1, 10, 100, 0.5, 0.5, 200, 1, 1);

            //generate random SEEDED list of 243 moves
            int[] moves = new int[243];
            Random rand = new Random(1);
            for (int i = 0; i < moves.Length; i++)
            {
                moves[i] = rand.Next(0, 7);
            }

            double score = robby.ComputeFitness(moves);
            
            Console.WriteLine("FITNESS: " + score);
            Console.WriteLine(string.Join("", moves));
            Assert.AreEqual(-1000, score);
        }

        [TestMethod]
        public void TestGeneratePossibleSolutions()
        {
            var robby = Robby.CreateRobby(200, 1, 10, 100, 0.5, 0.5, 200, 1, 1);

            //automatically create test directory
            string testOutputDirectory = "..\\..\\..\\GenerationBinTests";
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
    }
}
