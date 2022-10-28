using System.Diagnostics.CodeAnalysis;

namespace GeneticAlgorithm
{
  class Chromosome : IChromosome
  {
    public int this[int index] => throw new System.NotImplementedException();

    public double Fitness => throw new System.NotImplementedException();

    public int[] Genes => throw new System.NotImplementedException();

    public long Length => throw new System.NotImplementedException();

    public int CompareTo([AllowNull] IChromosome other)
    {
      throw new System.NotImplementedException();
    }

    public IChromosome[] Reproduce(IChromosome spouse, double mutationProb)
    {
      throw new System.NotImplementedException();
    }
  }
}