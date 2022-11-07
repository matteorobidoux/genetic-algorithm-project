using System;

namespace GeneticAlgorithm
{
  class GeneticAlgorithm : IGeneticAlgorithm
  {
    private int? _seed;
    private long _generationCount;
    private FitnessEventHandler _fitnessCalc;
    private Generation _currentGeneration;
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
      PopulationSize = populationSize;
      NumberOfGenes = numberOfGenes;
      LengthOfGene = lengthOfGenes;
      MutationRate = mutationRate;
      EliteRate = eliteRate;
      NumberOfTrials = numberOfTrials;
      _generationCount = 0;
      _fitnessCalc = calcFunction;
      _seed = seed;
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
      return _currentGeneration;
    }

    private Generation GenerateNextGeneration()
    {
      throw new NotImplementedException();
    }
  }
}