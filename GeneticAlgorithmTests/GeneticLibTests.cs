using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GeneticAlgorithm;

namespace GeneticAlgorithmTests
{
  [TestClass]
  public class GeneticLibTests
  {
    [TestMethod]
    public void TestCreator()
    {
      var alg = GeneticLib.CreateGeneticAlgorithm(10, 10, 7, .5, .5, 10, MockCalcFitness, 0);
      Assert.IsTrue(alg is GeneticAlgorithm.GeneticAlgorithm);
    }

    private double MockCalcFitness(IChromosome chromosome, IGeneration generation)
    {
      return chromosome[0];
    }
  }
}