using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GeneticAlgorithm;
using System.Diagnostics.CodeAnalysis;

namespace GeneticAlgorithmTests
{
  [TestClass]
  public class GenerationTests
  {
    [TestMethod]
    public void TestCtor()
    {
      var alg = new MockAlgorithm(10, 5, 10, 5);
      Generation g1 = new Generation(alg, MockCalcFitness, 0);
      int[] expectedGenes = new int[] {3, 4, 3, 2, 1, 2, 4, 2, 4, 1};
      Assert.AreEqual(10, g1.NumberOfChromosomes);
      for (int i = 0; i < g1.NumberOfChromosomes; i++)
      {
        for (int j = 0; j < g1[i].Length; j++)
        {
          Assert.AreEqual(expectedGenes[j], g1[i][j]);
        }
      }
    }

    [TestMethod]
    public void TestCopyCtor()
    {
      var alg = new MockAlgorithm(10, 5, 10, 5);
      Generation g1 = new Generation(alg, MockCalcFitness, 0);
      IChromosome[] chromes = new Chromosome[alg.PopulationSize];
      for (int i = 0; i < chromes.Length; i++)
      {
        chromes[i] = new Chromosome(alg.NumberOfGenes, alg.LengthOfGene, 0);
      }
      Generation g2 = new Generation(chromes, g1);

      int[] expectedGenes = new int[] {3, 4, 3, 2, 1, 2, 4, 2, 4, 1};
      Assert.AreEqual(10, g2.NumberOfChromosomes);
      for (int i = 0; i < g2.NumberOfChromosomes; i++)
      {
        for (int j = 0; j < g2[i].Length; j++)
        {
          Assert.AreEqual(expectedGenes[j], g2[i][j]);
        }
      }
    }

    [TestMethod]
    public void TestIndexerGet()
    {
      var alg = new MockAlgorithm(10, 5, 10, 5);
      Generation g1 = new Generation(alg, MockCalcFitness, 0);
      Assert.ThrowsException<IndexOutOfRangeException>(new Action(delegate {var x = g1[-1];}));
      Assert.ThrowsException<IndexOutOfRangeException>(new Action(delegate {var x = g1[10];}));
      Assert.IsNotNull(g1[0]);
      Assert.IsNotNull(g1[9]);
    }

    [TestMethod]
    public void TestEvaluateFitness()
    {
      var alg = new MockAlgorithm(10, 5, 10, 5);
      Generation g1 = new Generation(alg, MockCalcFitness, 0);
      int[] expectedGenes = new int[] {3, 4, 3, 2, 1, 2, 4, 2, 4, 1};
      g1.EvaluateFitnessOfPopulation();
      Assert.AreEqual(3,g1.MaxFitness, 0.000000001);
      Assert.AreEqual(3,g1.AverageFitness, 0.000000001);
      Assert.AreEqual(g1.MaxFitness, g1[0].Fitness);
      for (int i = 0; i < g1.NumberOfChromosomes; i++)
      {
        Assert.AreEqual(3, g1[i].Fitness, 0.000000001);
      }
    }
    

    private double MockCalcFitness(IChromosome chromosome, IGeneration generation)
    {
      return chromosome[0];
    }

    private class MockAlgorithm : IGeneticAlgorithm
    {
      public int PopulationSize {get;}

      public int NumberOfGenes {get;}

      public int LengthOfGene {get;}

      public MockAlgorithm(int numberOfGenes, int lengthOfGene, int populationSize, int numberOfTrials)
      {
        NumberOfGenes = numberOfGenes;
        LengthOfGene = lengthOfGene;
        PopulationSize = populationSize;
        NumberOfTrials = numberOfTrials;
      }

      public double MutationRate => throw new NotImplementedException();

      public double EliteRate => throw new NotImplementedException();

      public int NumberOfTrials {get;}

      public long GenerationCount => throw new NotImplementedException();

      public IGeneration CurrentGeneration => throw new NotImplementedException();

      public FitnessEventHandler FitnessCalculation => throw new NotImplementedException();

      public IGeneration GenerateGeneration()
      {
        throw new NotImplementedException();
      }
    }

  }

}
