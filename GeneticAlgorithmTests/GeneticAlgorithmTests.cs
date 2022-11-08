using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GeneticAlgorithm;

namespace GeneticAlgorithmTests
{
  [TestClass]
  public class GeneticAlgorithmTests
  {
    [TestMethod]
    public void TestCtor()
    {
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(0, 10, 7, .5, .5, 10, MockCalcFitness, 0);});
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(10, 0, 7, .5, .5, 10, MockCalcFitness, 0);});
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(10, 10, 0, .5, .5, 10, MockCalcFitness, 0);});
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(10, 10, 7, -1, .5, 10, MockCalcFitness, 0);});
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(10, 10, 7, 1.1, .5, 10, MockCalcFitness, 0);});
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(10, 10, 7, .5, -1, 10, MockCalcFitness, 0);});
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(10, 10, 7, .5, 1.1, 10, MockCalcFitness, 0);});
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(10, 10, 7, .5, .5, 0, MockCalcFitness, 0);});
      Assert.ThrowsException<ArgumentNullException>(() => {new GeneticAlgorithm.GeneticAlgorithm(10, 10, 7, .5, .5, 10, null, 0);});
      var alg = new GeneticAlgorithm.GeneticAlgorithm(10, 10, 7, .5, .5, 10, MockCalcFitness, 0);
      Assert.AreEqual(10, alg.PopulationSize);
      Assert.AreEqual(10, alg.NumberOfGenes);
      Assert.AreEqual(7, alg.LengthOfGene);
      Assert.AreEqual(0.5, alg.MutationRate);
      Assert.AreEqual(0.5, alg.EliteRate);
      Assert.AreEqual(10, alg.NumberOfTrials);
      Assert.AreEqual(MockCalcFitness, alg.FitnessCalculation);
      Assert.AreEqual(0, alg.GenerationCount);
      Assert.IsNull(alg.CurrentGeneration);
    }

    private double MockCalcFitness(IChromosome chromosome, IGeneration generation)
    {
      return chromosome[0];
    }
  }

}
