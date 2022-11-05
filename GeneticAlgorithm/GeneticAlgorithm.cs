namespace GeneticAlgorithm
{
  class GeneticAlgorithm : IGeneticAlgorithm
  {
    public int PopulationSize => throw new System.NotImplementedException();

    public int NumberOfGenes => throw new System.NotImplementedException();

    public int LengthOfGene => throw new System.NotImplementedException();

    public double MutationRate => throw new System.NotImplementedException();

    public double EliteRate => throw new System.NotImplementedException();

    public int NumberOfTrials => throw new System.NotImplementedException();

    public long GenerationCount => throw new System.NotImplementedException();

    public IGeneration CurrentGeneration => throw new System.NotImplementedException();

    public FitnessEventHandler FitnessCalculation => throw new System.NotImplementedException();

    public IGeneration GenerateGeneration()
    {
      throw new System.NotImplementedException();
    }
  }
}