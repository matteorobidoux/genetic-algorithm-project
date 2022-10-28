namespace GeneticAlgorithm
{
  internal class GenerationDetails : IGenerationDetails
  {
    public IChromosome this[int index] => throw new System.NotImplementedException();

    public double AverageFitness => throw new System.NotImplementedException();

    public double MaxFitness => throw new System.NotImplementedException();

    public long NumberOfChromosomes => throw new System.NotImplementedException();

    public void EvaluateFitnessOfPopulation()
    {
      throw new System.NotImplementedException();
    }

    public IChromosome SelectParent()
    {
      throw new System.NotImplementedException();
    }
  }
}