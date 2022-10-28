using System.Diagnostics.CodeAnalysis;
using System;

namespace GeneticAlgorithm
{
  class Chromosome : IChromosome
  {
    private int[] _genes;
    private int _lengthOfGenes;
    private int? _seed;

    public int this[int index]
    {
      get 
      {
        if (index < 0 || index >= Length) {
          throw new IndexOutOfRangeException($"Invalid index for gene. Expected between 0 and {Length}. Got: {index}");
        }
        return Genes[index];
      }
    }

    public double Fitness => throw new System.NotImplementedException();

    public int[] Genes => throw new System.NotImplementedException();

    public long Length => throw new System.NotImplementedException();

    public Chromosome(int numGenes, int lengthOfGenes, int? seed) 
    {
      if(numGenes <= 0) 
      {
        throw new ArgumentException($"The number of genes must be a positive integer. Got: {numGenes}");
      }
      if (lengthOfGenes <= 0)
      {
        throw new ArgumentException($"The length of a gene must be a positive integer. Got: {lengthOfGenes}");
      }
      this._genes = new int[numGenes];
      this._lengthOfGenes = lengthOfGenes;
      this._seed = seed;
    }

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