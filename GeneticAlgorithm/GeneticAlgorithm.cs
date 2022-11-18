using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
  class GeneticAlgorithm : IGeneticAlgorithm
  {
    private int? _seed;
    private long _generationCount;
    private FitnessEventHandler _fitnessCalc;
    private Generation _currentGeneration;
    private int _eliteNum;
    public int PopulationSize { get; }

    public int NumberOfGenes { get; }

    public int LengthOfGene { get; }

    public double MutationRate { get; }

    public double EliteRate { get; }

    public int NumberOfTrials { get; }

    public long GenerationCount { get => _generationCount; }

    public IGeneration CurrentGeneration { get => _currentGeneration; }

    public FitnessEventHandler FitnessCalculation { get => _fitnessCalc; }

    internal GeneticAlgorithm(int populationSize, int numberOfGenes, int lengthOfGenes, double mutationRate,
     double eliteRate, int numberOfTrials, FitnessEventHandler calcFunction, int? seed = null)
    {
      Debug.Assert(populationSize >= 10, "Don't know how this got past validation");
      Debug.Assert(numberOfGenes > 0, "Don't know how this got past validation");
      Debug.Assert(lengthOfGenes > 0, "Don't know how this got past validation");
      Debug.Assert(eliteRate > 0 && eliteRate < 1, "Don't know how this got past validation");
      Debug.Assert(populationSize % 2 == 0 && eliteRate * populationSize >= 2 || populationSize % 2 == 1 && eliteRate * populationSize >= 3, "Don't know how this got past validation");
      Debug.Assert(mutationRate >= 0 && mutationRate <= 1, "Don't know how this got past validation");
      Debug.Assert(numberOfTrials > 0, "Don't know how this got past validation");
      Debug.Assert(calcFunction != null, "Don't know how this got past validation");
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

      Debug.Assert(_eliteNum % 2 == populationSize % 2, "Something went wrong with the above code");

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
      Debug.Assert(_currentGeneration != null, "How is it even possible for it to be null?");
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
      Debug.Assert(_currentGeneration != null, "This probably shouldn't be running");
      for (int i = 0; i < elites.Length; i++)
      {
        elites[i] = _currentGeneration[i];
        nextGeneration[i] = _currentGeneration[i];
      }
      Generation eliteGen = new Generation(elites, _currentGeneration);
      //Fill the remaining population with children
      Parallel.For(elites.Length, PopulationSize - 1, i =>
      {
        if (i % 2 == PopulationSize % 2)
        {
          IChromosome parent1 = eliteGen.SelectParent();
          IChromosome parent2;
          int tries = 0;
          //Try and prevent inbreeding
          do
          {
            parent2 = eliteGen.SelectParent();
            tries++;
          }
          while (_seed == null && parent1 == parent2 && tries < 42);
          IChromosome[] children = parent1.Reproduce(parent2, MutationRate);
          nextGeneration[i] = children[0];
          nextGeneration[i + 1] = children[1];
        }
      });

      return new Generation(nextGeneration, _currentGeneration);
    }
  }
}