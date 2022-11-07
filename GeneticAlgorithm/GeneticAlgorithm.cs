using System;

namespace GeneticAlgorithm
{
  class GeneticAlgorithm : IGeneticAlgorithm
  {
    private int? _seed;
    private long _generationCount;
    private FitnessEventHandler _fitnessCalc;
    private Generation _currentGeneration;
    private int _eliteNum;
    private int _elites;
    public int PopulationSize {get;}

    public int NumberOfGenes {get;}

    public int LengthOfGene {get;}

    public double MutationRate {get;}

    public double EliteRate {get;}

    public int NumberOfTrials {get;}

    public long GenerationCount {get => _generationCount;}

    public IGeneration CurrentGeneration {get => _currentGeneration;}

    public FitnessEventHandler FitnessCalculation {get => _fitnessCalc;}

    internal GeneticAlgorithm(int populationSize, int numberOfGenes, int lengthOfGenes, double mutationRate,
     double eliteRate, int numberOfTrials, FitnessEventHandler calcFunction, int? seed = null) 
    {
      if (populationSize < 10)
      {
        throw new ArgumentOutOfRangeException($"Population minimum is 10. Got: {populationSize}");
      }
      if(numberOfGenes <= 0) 
      {
        throw new ArgumentOutOfRangeException($"The number of genes must be a positive integer. Got: {numberOfGenes}");
      }
      if (lengthOfGenes <= 0)
      {
        throw new ArgumentOutOfRangeException($"The length of a gene must be a positive integer. Got: {lengthOfGenes}");
      }
      if (eliteRate < 0 || eliteRate > 1)
      {
        throw new ArgumentOutOfRangeException($"Elite rate expected between 0 and 1. Got: {eliteRate}");
      }
      if (mutationRate < 0 || mutationRate > 1)
      {
        throw new ArgumentOutOfRangeException($"Mutation rate expected between 0 and 1. Got: {mutationRate}");
      }
      if (numberOfTrials <= 0)
      {
        throw new ArgumentOutOfRangeException($"Minimum number of trials is 1. Got: {numberOfTrials}");
      }
      if (calcFunction == null)
      {
        throw new ArgumentNullException($"The fitness calculation delegate cannot be null");
      }
      PopulationSize = populationSize;
      NumberOfGenes = numberOfGenes;
      LengthOfGene = lengthOfGenes;
      MutationRate = mutationRate;
      EliteRate = eliteRate;
      NumberOfTrials = numberOfTrials;
      _generationCount = 0;
      _fitnessCalc = calcFunction;
      _seed = seed;

      _eliteNum = (int)Math.Ceiling(eliteRate * populationSize);
      if (_eliteNum % 2 != 0)
      {
        _eliteNum--;
      }
      
    }

    public IGeneration GenerateGeneration()
    {
      if (_currentGeneration == null)
      {
        _currentGeneration = new Generation(this, _fitnessCalc, _seed);
      }
      else
      {
        _currentGeneration = GenerateNextGeneration();
      }
      _generationCount++;
      _currentGeneration.EvaluateFitnessOfPopulation();
      return _currentGeneration;
    }

    private Generation GenerateNextGeneration()
    {
      Random rand = _seed == null ? new Random() : new Random((int)_seed);
      IChromosome[] elites = new IChromosome[_eliteNum];
      IChromosome[] nextGeneration = new IChromosome[PopulationSize];
      for (int i = 0; i < elites.Length; i++)
      {
        IChromosome elite = _currentGeneration.SelectParent();
        elites[i] = elite;
        nextGeneration[i] = elite;
      }
      for (int i = elites.Length; i < PopulationSize - 1; i += 2)
      {
        int parent1 = rand.Next(elites.Length);
        int parent2 = rand.Next(elites.Length);
        IChromosome[] children = elites[parent1].Reproduce(elites[parent2], MutationRate);
        nextGeneration[i] = children[0];
        nextGeneration[i + 1] = children[1];
      }

      return new Generation(nextGeneration, _currentGeneration);
    }
  }
}