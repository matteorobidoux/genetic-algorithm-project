using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobbyTheRobot;
using System;

namespace TestRobbyTheRobot
{
    [TestClass]
    public class TestRobbyTheRobot
    {
        [TestMethod]
        public void TestGenerateRandomTestGridFiftyFifty()
        {
            var robby = new RobbyTheRobot.RobbyTheRobot(10, 0, 0, 0);
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
            
            //Expects 50% from the gridsize
            int expected = (int) Math.Round(robby.GridSize * robby.GridSize * 0.5);

            Assert.AreEqual(expected, numOfCans);
            Assert.AreEqual(expected, numOfEmpties);
        }
    }
}
