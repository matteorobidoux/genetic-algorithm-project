using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;
using System.Diagnostics;

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
        return _genes[index];
      }
      internal set
      {
        Debug.Assert(index >= 0 && index < Length, "Please tell me you remember how indexes work?");
        Debug.Assert(value >= 0 && value < _lengthOfGenes, "Your new gene value must conform to the gene length");
        _genes[index] = value;
      }
    }

    public double Fitness {get; internal set;}

    public int[] Genes => _genes;

    public long Length => _genes.Length;

    internal Chromosome(int numGenes, int lengthOfGenes, int? seed = null) 
    {
      Debug.Assert(numGenes > 0, "You messed up when constructing the number of genes");
      Debug.Assert(lengthOfGenes > 0, "You need a longer length of gene");
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
      Debug.Assert(genes != null, "You can't give null genes");
      Debug.Assert(lengthOfGenes > 0, "Copying still needs good gene lengths");
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
      Debug.Assert(_genes != null, "How did this even happen?");
      Debug.Assert(_lengthOfGenes >= 0, "How did this even happen?");
      Chromosome parent = spouse as Chromosome;
      Chromosome[] children = new Chromosome[]{new Chromosome(_genes, _lengthOfGenes, _seed), 
      new Chromosome(parent._genes, parent._lengthOfGenes, parent._seed)};
      
      Random rand = GetRandomObj();
      int start = rand.Next((int)Length);
      int end = rand.Next(start, (int)Length);
      Cross(children[0], children[1], start, end);
      Mutate(children, mutationProb);
      Debug.Assert(children.Length == 2, "I have no idea how it isn't 2");

      return children;
    }

    // <summary>
    // Given an array of Chromosomes, this will go through each gene of each Chromosome and mutate it
    // according to the mutation probability
    // </summary>
    private void Mutate(Chromosome[] children, double mutationProb)
    {
      Debug.Assert(children != null, "Gotta have children to mutate");
      Debug.Assert(mutationProb >= 0 && mutationProb <= 1, "Your mutation chance is wrong");
      Debug.Assert(_genes != null, "How did this even happen?");
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
      Debug.Assert(start <= end, "You may have accidentally swapped the two points");
      Debug.Assert(childA != null & childB != null, "Neither of the children can be null");
      for (int i = start; i <= end; i++) {
        int temp = childA[i];
        childA[i] = childB[i];
        childB[i] = temp;
      }
    }
  }
}