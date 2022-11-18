using System;

namespace GeneticAlgorithm
{
  public static class GeneticLib
  {
    public static IGeneticAlgorithm CreateGeneticAlgorithm(int populationSize, int numberOfGenes, int lengthOfGene, double mutationRate, double eliteRate, int numberOfTrials, FitnessEventHandler fitnessCalculation, int? seed = null)
    {
      if (populationSize < 10)
      {
        throw new ArgumentOutOfRangeException($"Population minimum is 10. Got: {populationSize}");
      }
      else if (numberOfGenes <= 0)
      {
        throw new ArgumentOutOfRangeException($"The number of genes must be a positive integer. Got: {numberOfGenes}");
      }
      else if (lengthOfGene <= 0)
      {
        throw new ArgumentOutOfRangeException($"The length of a gene must be a positive integer. Got: {lengthOfGene}");
      }
      else if (eliteRate <= 0 || eliteRate >= 1)
      {
        throw new ArgumentOutOfRangeException($"Elite rate expected between 0 and 1. Got: {eliteRate}");
      }
      else if (populationSize % 2 == 0 && eliteRate * populationSize < 2 || populationSize % 2 == 1 && eliteRate * populationSize < 3)
      {
        throw new ArgumentOutOfRangeException($"Elite rate must provide at least 2 or 3 parents depending on the population size");
      }
      else if (mutationRate < 0 || mutationRate > 1)
      {
        throw new ArgumentOutOfRangeException($"Mutation rate expected between 0 and 1. Got: {mutationRate}");
      }
      else if (numberOfTrials <= 0)
      {
        throw new ArgumentOutOfRangeException($"Minimum number of trials is 1. Got: {numberOfTrials}");
      }
      else if (fitnessCalculation == null)
      {
        throw new ArgumentNullException($"The fitness calculation delegate cannot be null");
      }
      else
      {
        return new GeneticAlgorithm(populationSize, numberOfGenes, lengthOfGene, mutationRate, eliteRate, numberOfTrials, fitnessCalculation, seed);
      }
    }
  }
}