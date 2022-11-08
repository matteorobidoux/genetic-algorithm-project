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
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(10, 10, 7, .5, .19, 10, MockCalcFitness, 0);});
      Assert.ThrowsException<ArgumentOutOfRangeException>(() => {new GeneticAlgorithm.GeneticAlgorithm(11, 10, 7, .5, .19, 10, MockCalcFitness, 0);});
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

    [TestMethod]
    public void TestGenerateGeneration()
    {
      var alg = new GeneticAlgorithm.GeneticAlgorithm(10, 10, 5, .5, .5, 5, MockCalcFitness, 0);
      alg.GenerateGeneration();
      Assert.AreEqual(1, alg.GenerationCount);
      Generation g1 = alg.CurrentGeneration as Generation;
      int[] expectedGenes = new int[] {3, 4, 3, 2, 1, 2, 4, 2, 4, 1};
      Assert.AreEqual(10, g1.NumberOfChromosomes);
      for (int i = 0; i < g1.NumberOfChromosomes; i++)
      {
        Assert.AreEqual(10, g1[i].Length);
        for (int j = 0; j < g1[i].Length; j++)
        {
          Assert.AreEqual(expectedGenes[j], g1[i][j]);
        }
      }
    }

    [TestMethod]
    public void TestGenerateGenerationMultipleEvenPop()
    {
      var alg = new GeneticAlgorithm.GeneticAlgorithm(10, 10, 5, .5, .5, 5, MockCalcFitness, 0);
      alg.GenerateGeneration();
      alg.GenerateGeneration();
      Assert.AreEqual(2, alg.GenerationCount);
      Generation g1 = alg.CurrentGeneration as Generation;
      int[] expectedGenes = new int[] {3, 4, 3, 2, 1, 2, 4, 2, 4, 1};
      int[] expectedKid1 = new int[] { 3, 4, 2, 4, 3, 4, 4, 2, 3, 1 };
      int[] expectedKid2 = new int[] { 3, 4, 3, 1, 4, 2, 4, 2, 4, 1 };
      Assert.AreEqual(10, g1.NumberOfChromosomes);
      for (int i = 0; i < g1.NumberOfChromosomes; i++)
      {
        Assert.AreEqual(10, g1[i].Length);
        if (i < 4)
        {
          for (int j = 0; j < g1[i].Length; j++)
          {
            Assert.AreEqual(expectedGenes[j], g1[i][j]);
          }
        }
        else if (i % 2 == 0)
        {
          for (int j = 0; j < g1[i].Length; j++)
          {
            Assert.AreEqual(expectedKid1[j], g1[i][j]);
          }
        }
        else
        {
          for (int j = 0; j < g1[i].Length; j++)
          {
            Assert.AreEqual(expectedKid2[j], g1[i][j]);
          }
        }
      }
    }

    [TestMethod]
    public void TestGenerateGenerationMultipleOddPop()
    {
      var alg = new GeneticAlgorithm.GeneticAlgorithm(11, 10, 5, .5, .5, 5, MockCalcFitness, 0);
      alg.GenerateGeneration();
      alg.GenerateGeneration();
      Assert.AreEqual(2, alg.GenerationCount);
      Generation g1 = alg.CurrentGeneration as Generation;
      int[] expectedGenes = new int[] {3, 4, 3, 2, 1, 2, 4, 2, 4, 1};
      int[] expectedKid1 = new int[] { 3, 4, 2, 4, 3, 4, 4, 2, 3, 1 };
      int[] expectedKid2 = new int[] { 3, 4, 3, 1, 4, 2, 4, 2, 4, 1 };
      Assert.AreEqual(11, g1.NumberOfChromosomes);
      for (int i = 0; i < g1.NumberOfChromosomes; i++)
      {
        Assert.AreEqual(10, g1[i].Length);
        if (i < 5)
        {
          for (int j = 0; j < g1[i].Length; j++)
          {
            Assert.AreEqual(expectedGenes[j], g1[i][j]);
          }
        }
        else if (i % 2 != 0)
        {
          for (int j = 0; j < g1[i].Length; j++)
          {
            Assert.AreEqual(expectedKid1[j], g1[i][j]);
          }
        }
        else
        {
          for (int j = 0; j < g1[i].Length; j++)
          {
            Assert.AreEqual(expectedKid2[j], g1[i][j]);
          }
        }
      }
    }

    private double MockCalcFitness(IChromosome chromosome, IGeneration generation)
    {
      return chromosome[0];
    }
  }

}
