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

    public int[] Genes => _genes;

    public long Length => _genes.Length;

    internal Chromosome(int numGenes, int lengthOfGenes, int? seed) 
    {
      if(numGenes <= 0) 
      {
        throw new ArgumentException($"The number of genes must be a positive integer. Got: {numGenes}");
      }
      if (lengthOfGenes <= 0)
      {
        throw new ArgumentException($"The length of a gene must be a positive integer. Got: {lengthOfGenes}");
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

    private Random GetRandomObj()
    {
      return _seed == null ? new Random() : new Random((int)_seed);
    }

    internal Chromosome(Chromosome original)
    {
      _seed = original._seed;
      _lengthOfGenes = original._lengthOfGenes;
      _genes = new int[original.Length];
      Array.Copy(original._genes, _genes, original.Length);
      Fitness = original.Fitness;
    }

    public int CompareTo([AllowNull] IChromosome other)
    {
      if(other == null) return 1;
      if(Fitness == other.Fitness) return 0;
      return Fitness > other.Fitness? -1 : 1;
    }

    public IChromosome[] Reproduce(IChromosome spouse, double mutationProb)
    {
      if(spouse !is Chromosome || spouse.Length != Length || (spouse as Chromosome)._lengthOfGenes != _lengthOfGenes)
      {
        throw new ArgumentException("The spouse is incompatible with this chromosome");
      }
      if(mutationProb < 0 || mutationProb > 1)
      {
        throw new ArgumentOutOfRangeException($"Invalid mutationProb. Expected value between 0 and 1. Got: {mutationProb}");
      }
      IChromosome[] children = Crossover(spouse as Chromosome);
      Random rand = GetRandomObj();
      for (int i = 0; i < Length; i++)
      {
        foreach (var child in children)
        {
          double prob = rand.NextDouble();
          if (prob <= mutationProb)
          {
            child.Genes[i] = rand.Next(_lengthOfGenes);
          }
        }
      }
      return children;
    }

    private Chromosome[] Crossover(Chromosome spouce) 
    {
      Random rand = GetRandomObj();
      int start = rand.Next((int)Length);
      int end = rand.Next(start, (int)Length);
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