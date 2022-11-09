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
      if (populationSize % 2 == 0 && eliteRate * populationSize < 2 || populationSize % 2 == 1 && eliteRate * populationSize < 3)
      {
        throw new ArgumentOutOfRangeException($"Elite rate must provide at least 2 or 3 parents depending on the population size");
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
      //Want to leave room for 2 children being added to the new population
      if (_eliteNum % 2 != populationSize % 2)
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

    /// <summary>
    /// Generates a new generation based on the previous generation and the elite rate
    /// </summary>
    /// <return> The newly generated generation </return>
    private Generation GenerateNextGeneration()
    {
      Random rand = _seed == null ? new Random() : new Random((int)_seed);
      IChromosome[] elites = new Chromosome[_eliteNum];
      IChromosome[] nextGeneration = new Chromosome[PopulationSize];
      for (int i = 0; i < elites.Length; i++)
      {
        elites[i] = _currentGeneration[i];
        nextGeneration[i] = _currentGeneration[i];
      }
      Generation eliteGen = new Generation(elites, _currentGeneration);
      for (int i = elites.Length; i < PopulationSize - 1; i += 2)
      {
        IChromosome parent1 = eliteGen.SelectParent();
        IChromosome parent2;
        do 
        {
          parent2 = eliteGen.SelectParent();
        }
        while(_seed == null && parent1 == parent2);
        IChromosome[] children = parent1.Reproduce(parent2, MutationRate);
        nextGeneration[i] = children[0];
        nextGeneration[i + 1] = children[1];
      }

      return new Generation(nextGeneration, _currentGeneration);
    }
  }
}