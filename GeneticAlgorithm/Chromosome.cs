using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;

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
        if (index < 0 || index >= Length) 
        {
          throw new IndexOutOfRangeException($"Invalid index for gene. Expected between 0 and {Length}. Got: {index}");
        }
        return Genes[index];
      }
      internal set
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

    public double Fitness {get; internal set;}

    public int[] Genes => _genes.ToArray();

    public long Length => _genes.Length;

    internal Chromosome(int numGenes, int lengthOfGenes, int? seed = null) 
    {
      if(numGenes <= 0) 
      {
        throw new ArgumentOutOfRangeException($"The number of genes must be a positive integer. Got: {numGenes}");
      }
      if (lengthOfGenes <= 0)
      {
        throw new ArgumentOutOfRangeException($"The length of a gene must be a positive integer. Got: {lengthOfGenes}");
      }
      _seed = seed;
      Random rand = GetRandomObj();
      _lengthOfGenes = lengthOfGenes;
      _genes = new int[numGenes];
      for (int i = 0; i < Length; i++)
      {
        _genes[i] = rand.Next(0, _lengthOfGenes);
      }
    }

    // <summary>
    // Makes a Random object, either seeded or not
    // </summary>
    // <return>The Random object</return>
    private Random GetRandomObj()
    {
      return _seed == null ? new Random() : new Random((int)_seed);
    }

    internal Chromosome(int[] genes, int lengthOfGenes, int? seed = null)
    {
      _seed = seed;
      _lengthOfGenes = lengthOfGenes;
      _genes = genes.ToArray();
    }

    public int CompareTo([AllowNull] IChromosome other)
    {
      if(other == null) return 1;
      if(Fitness == other.Fitness) return 0;
      return Fitness > other.Fitness? -1 : 1;
    }

    public IChromosome[] Reproduce(IChromosome spouse, double mutationProb)
    {
      if(!(spouse is Chromosome) || spouse.Length != Length || (spouse as Chromosome)._lengthOfGenes != _lengthOfGenes)
      {
        throw new ArgumentException("The spouse is incompatible with this chromosome");
      }
      if(mutationProb < 0 || mutationProb > 1)
      {
        throw new ArgumentOutOfRangeException($"Invalid mutationProb. Expected value between 0 and 1. Got: {mutationProb}");
      }
      Chromosome parent = spouse as Chromosome;
      Chromosome[] children = new Chromosome[]{new Chromosome(_genes, _lengthOfGenes, _seed), 
      new Chromosome(parent._genes, parent._lengthOfGenes, parent._seed)};
      
      Random rand = GetRandomObj();
      int start = rand.Next((int)Length);
      int end = rand.Next(start, (int)Length);
      Cross(children[0], children[1], start, end);
      Mutate(children, mutationProb);

      return children;
    }

    // <summary>
    // Given an array of Chromosomes, this will go through each gene of each Chromosome and mutate it
    // according to the mutation probability
    // </summary>
    private void Mutate(Chromosome[] children, double mutationProb)
    {
      Random rand = GetRandomObj();
      for (int i = 0; i < Length; i++)
      {
        foreach (var child in children)
        {
          double prob = rand.NextDouble();
          if (prob < mutationProb)
          {
            child._genes[i] = rand.Next(_lengthOfGenes);
          }
        }
      }
    }

    // <summary>
    // Crosses two chromosomes. Given a starting point and an end point, this method will take the genes of the
    // given chromosomes and swap the genes in the given range.
    // </summary>
    private void Cross(Chromosome childA, Chromosome childB, int start, int end)
    {
      for (int i = start; i <= end; i++) {
        int temp = childA[i];
        childA[i] = childB[i];
        childB[i] = temp;
      }
    }
  }
}