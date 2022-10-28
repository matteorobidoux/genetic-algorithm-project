using System.Diagnostics.CodeAnalysis;

namespace GeneticAlgorithm
{
  class Chromosome : IChromosome
  {
    public int this[int index]
    {
      get 
      {
        if (index < 0 || index >= Length) {
          throw new System.IndexOutOfRangeException($"Invalid index for gene. Expected between 0 and {Length}. Got: {index}");
        }
        return Genes[index];
      }
    }

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