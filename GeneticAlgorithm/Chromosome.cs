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

    public double Fitness {get; set;}

    public int[] Genes => _genes;

    public long Length => _genes.Length;

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
      _genes = new int[numGenes];
      _lengthOfGenes = lengthOfGenes;
      _seed = seed;
    }

    public Chromosome(Chromosome original)
    {
      _seed = original._seed;
      _lengthOfGenes = original._lengthOfGenes;
      Array.Copy(original._genes, _genes, original.Length);
      Fitness = original.Fitness;
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