using System;
using System.Diagnostics;

namespace GeneticAlgorithm
{
  internal class Generation : IGenerationDetails
  {
    private IGeneticAlgorithm _alg;
    private FitnessEventHandler _fitnessCalc;
    private int? _seed;
    private Chromosome[] _generation;
    public IChromosome this[int index]
    {
      get
      {
        if (index < 0 || index >= NumberOfChromosomes)
        {
          throw new IndexOutOfRangeException($"Invalid index for Generation. Expected between 0 and {NumberOfChromosomes}. Got: {index}");
        }
        return _generation[index];
      }
    }

    public double AverageFitness { get; private set; }

    public double MaxFitness { get; private set; }
    public long NumberOfChromosomes { get => _generation.Length; }

    internal Generation(IGeneticAlgorithm alg, FitnessEventHandler fitnessCalc, int? seed = null)
    {
      Debug.Assert(alg != null, "A generation needs an algorithm");
      Debug.Assert(fitnessCalc != null, "A generation needs a way to calculate its fitness");
      _fitnessCalc = fitnessCalc;
      _seed = seed;
      _alg = alg;
      _generation = new Chromosome[_alg.PopulationSize];
      for (int i = 0; i < NumberOfChromosomes; i++)
      {
        _generation[i] = new Chromosome(_alg.NumberOfGenes, _alg.LengthOfGene, _seed);
      }
    }

    internal Generation(IChromosome[] chromosomes, Generation generation)
    {

      Debug.Assert(chromosomes != null, "This needs chromosomes to work");
      Debug.Assert(chromosomes is Chromosome[], "This only works with the Chromosome class");
      Debug.Assert(generation != null, "Needs a generation for details");
      _fitnessCalc = generation._fitnessCalc;
      _seed = generation._seed;
      _alg = generation._alg;
      _generation = chromosomes as Chromosome[];
      foreach (var chromosome in chromosomes)
      {
        AverageFitness += chromosome.Fitness / chromosomes.Length;
      }
    }

    public void EvaluateFitnessOfPopulation()
    {
      double totalFitness = 0;
      Debug.Assert(_generation != null && _alg != null, "Is your constructor ok?");
      foreach (Chromosome chromosome in _generation)
      {
        for (int i = 0; i < _alg.NumberOfTrials; i++)
        {
          double fitness = _fitnessCalc(chromosome, this) / _alg.NumberOfTrials;
          //reset the fitness if a fitness already exists
          if (i == 0)
          {
            chromosome.Fitness = 0;
          }
          chromosome.Fitness += fitness;
          totalFitness += fitness;
        }
        if (chromosome == _generation[0])
        {
          MaxFitness = chromosome.Fitness;
        }
        if (chromosome.Fitness > MaxFitness)
        {
          MaxFitness = chromosome.Fitness;
        }
      }
      AverageFitness = totalFitness / NumberOfChromosomes;
      Array.Sort(_generation);

      //Delta to deal with any rounding errors
      Debug.Assert(MaxFitness >= AverageFitness - 0.0000001, "Is this even mathematically possible?");
      Debug.Assert(_generation[0].Fitness == MaxFitness, "Somehow the sorting broke");
    }

    public IChromosome SelectParent()
    {
      //Could have a problem/crash if the fitnesses are all negative, or the average is 0
      //If the average is positive and there are some negatives, then they will never be chosen
      Random rand = _seed == null ? new Random() : new Random((int)_seed);
      //Weigthed selection algorithm only works with positive fitnesses
      if (AverageFitness > 0)
      {
        double pool = rand.NextDouble() * (AverageFitness * NumberOfChromosomes);
        int index;
        for (index = 0; index < NumberOfChromosomes; index++)
        {
          pool -= _generation[index].Fitness;
          if (pool <= 0)
          {
            break;
          }
        }
        return _generation[index];
      }
      else
      {
        return _generation[rand.Next(_generation.Length)];
      }
    }
  }
}