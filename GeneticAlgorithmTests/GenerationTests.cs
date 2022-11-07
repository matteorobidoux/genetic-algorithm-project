using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GeneticAlgorithm;

namespace GeneticAlgorithmTests
{
  [TestClass]
  public class GenerationTests
  {
    [TestMethod]
    public void TestCtor()
    {
      var alg = new MockAlgorithm(10, 5, 10, 5);
      Assert.ThrowsException<ArgumentNullException>(()=>{new Generation(null, MockCalcFitness);});
      Assert.ThrowsException<ArgumentNullException>(()=>{new Generation(alg, null);});
      Generation g1 = new Generation(alg, MockCalcFitness, 0);
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
    public void TestRandomCtor()
    {
      var alg = new MockAlgorithm(100, 11, 100, 10);
      Generation g1 = new Generation(alg, MockCalcFitness);
      Assert.AreEqual(100, g1.NumberOfChromosomes);
      for (int i = 0; i < g1.NumberOfChromosomes; i++)
      {
        Assert.AreEqual(100, g1[i].Length);
        for (int j = 0; j < g1[i].Length; j++)
        {
          Assert.AreEqual(5, g1[i][j], 5);
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

      Assert.ThrowsException<ArgumentNullException>(()=>{new Generation(null, g1);});
      Assert.ThrowsException<ArgumentNullException>(()=>{new Generation(new Chromosome[10], null);});
      Generation g2 = new Generation(chromes, g1);

      int[] expectedGenes = new int[] {3, 4, 3, 2, 1, 2, 4, 2, 4, 1};
      Assert.AreEqual(10, g2.NumberOfChromosomes);
      for (int i = 0; i < g2.NumberOfChromosomes; i++)
      {
        Assert.AreEqual(10, g1[i].Length);
        for (int j = 0; j < g2[i].Length; j++)
        {
          Assert.AreEqual(expectedGenes[j], g2[i][j]);
        }
      }
    }

    [TestMethod]
    public void TestRandomCopyCtor()
    {
      var alg = new MockAlgorithm(100, 11, 100, 10);
      Generation g1 = new Generation(alg, MockCalcFitness);
      IChromosome[] chromes = new Chromosome[alg.PopulationSize];
      for (int i = 0; i < chromes.Length; i++)
      {
        chromes[i] = new Chromosome(alg.NumberOfGenes, alg.LengthOfGene);
      }
      Generation g2 = new Generation(chromes, g1);
      Assert.AreEqual(100, g2.NumberOfChromosomes);
      for (int i = 0; i < g2.NumberOfChromosomes; i++)
      {
        Assert.AreEqual(100, g2[i].Length);
        for (int j = 0; j < g2[i].Length; j++)
        {
          Assert.AreEqual(5, g2[i][j], 5);
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
      Assert.IsTrue(g1[0] is Chromosome);
      Assert.IsTrue(g1[9] is Chromosome);
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

    [TestMethod]
    public void TestRandomEvaluateFitness()
    {
      var alg = new MockAlgorithm(100, 11, 100, 10);
      Generation g1 = new Generation(alg, MockCalcFitness);
      g1.EvaluateFitnessOfPopulation();
      Assert.AreEqual(5,g1.MaxFitness, 5);
      Assert.AreEqual(5,g1.AverageFitness, 5);
      Assert.AreEqual(g1.MaxFitness, g1[0].Fitness);
      for (int i = 0; i < g1.NumberOfChromosomes; i++)
      {
        Assert.AreEqual(5, g1[i].Fitness, 5);
      }
    }

    [TestMethod]
    public void TestSelectParent()
    {
      var alg = new MockAlgorithm(10, 5, 10, 5);
      Generation g1 = new Generation(alg, MockCalcFitness, 0);
      int[] expectedGenes = new int[] {3, 4, 3, 2, 1, 2, 4, 2, 4, 1};
      g1.EvaluateFitnessOfPopulation();
      Assert.AreEqual(g1[7], g1.SelectParent());
    }

    [TestMethod]
    public void TestRandomSelectParent()
    {
      var alg = new MockAlgorithm(10, 5, 10, 10);
      Generation g1 = new Generation(alg, MockCalcFitness);
      g1.EvaluateFitnessOfPopulation();
      Assert.IsTrue(g1.SelectParent() is Chromosome);
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
