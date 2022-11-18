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
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(0, 10, 7, .5, .5, 10, MockCalcFitness, 0); });
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(10, 0, 7, .5, .5, 10, MockCalcFitness, 0); });
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(10, 10, 0, .5, .5, 10, MockCalcFitness, 0); });
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(10, 10, 7, -1, .5, 10, MockCalcFitness, 0); });
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(10, 10, 7, 1.1, .5, 10, MockCalcFitness, 0); });
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(10, 10, 7, .5, -1, 10, MockCalcFitness, 0); });
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(10, 10, 7, .5, 1.1, 10, MockCalcFitness, 0); });
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(10, 10, 7, .5, .5, 0, MockCalcFitness, 0); });
      Assert.ThrowsException<ArgumentNullException>(() => { GeneticLib.CreateGeneticAlgorithm(10, 10, 7, .5, .5, 10, null, 0); });
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(10, 10, 7, .5, .19, 10, MockCalcFitness, 0); });
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => { GeneticLib.CreateGeneticAlgorithm(11, 10, 7, .5, .19, 10, MockCalcFitness, 0); });
      var alg = GeneticLib.CreateGeneticAlgorithm(10, 10, 7, .5, .5, 10, MockCalcFitness, 0);
      Assert.IsTrue(alg is GeneticAlgorithm.GeneticAlgorithm);
    }

    private double MockCalcFitness(IChromosome chromosome, IGeneration generation)
    {
      return chromosome[0];
    }
  }
}