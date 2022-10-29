using System.Diagnostics.CodeAnalysis;
using System;

namespace GeneticAlgorithm
{
  class Chromosome : IChromosome
  {
    private int[] _genes;
    private int _lengthOfGenes;
    private int? _seed;
    private Random _rand;

    public int this[int index]
    {
      get 
      {
        if (index < 0 || index >= Length) 
        {
          throw new IndexOutOfRangeException($"Invalid index for gene. Expected between 0 and {Length}. Got: {index}");
        }
        return Genes[index];
      }
      set
      {
        if (index < 0 || index >= Length) 
        {
          throw new IndexOutOfRangeException($"Invalid index for gene. Expected between 0 and {Length}. Got: {index}");
        }
        if (value < 0 || value > _lengthOfGenes)
        {
          throw new ArgumentOutOfRangeException($"Invalid gene length. Expected between 0 and {_lengthOfGenes}. Got: {value}");
        }
        _genes[index] = value;
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
      _rand = _seed == null ? new Random() : new Random((int)_seed);
    }

    public Chromosome(Chromosome original)
    {
      _seed = original._seed;
      _lengthOfGenes = original._lengthOfGenes;
      Array.Copy(original._genes, _genes, original.Length);
      Fitness = original.Fitness;
      _rand = _seed == null ? new Random() : new Random((int)_seed);
    }

    public int CompareTo([AllowNull] IChromosome other)
    {
      if(other == null) return 1;
      if(Fitness == other.Fitness) return 0;
      if(Fitness > other.Fitness)
      {
        return -1;
      }
      else
      {
        return 1;
      }
    }

    public IChromosome[] Reproduce(IChromosome spouse, double mutationProb)
    {
      throw new System.NotImplementedException();
    }

    private Chromosome[] Crossover(Chromosome spouce) 
    {
      int start = _rand.Next((int)Length);
      int end = _rand.Next(start, (int)Length);
      Chromosome[] children = new Chromosome[]{new Chromosome(this), new Chromosome(spouce)};
      for (int i = start; i < end; i++) {
        int temp = children[0][i];
        children[0][i] = children[1][i];
        children[1][i] = temp;
      }

      return children;
    } 
  }
}