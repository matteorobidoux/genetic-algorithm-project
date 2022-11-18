using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

    private double? _averageFitness;
    public double AverageFitness
    {
      get
      {
        if (_averageFitness == null)
        {
          _averageFitness = _generation.Average<Chromosome>((chromosome) => { return chromosome.Fitness; });
        }
        return (double)_averageFitness;
      }
    }

    private double? _maxFitness;
    public double MaxFitness
    {
      get
      {
        if (_maxFitness == null)
        {
          _maxFitness = _generation.Max<Chromosome>((chromosome) => { return chromosome.Fitness; });
        }
        return (double)_maxFitness;
      }
    }
    public long NumberOfChromosomes { get => _generation.Length; }

    internal Generation(IGeneticAlgorithm alg, FitnessEventHandler fitnessCalc, int? seed = null)
    {
      Debug.Assert(alg != null, "A generation needs an algorithm");
      Debug.Assert(fitnessCalc != null, "A generation needs a way to calculate its fitness");
      _fitnessCalc = fitnessCalc;
      _seed = seed;
      _alg = alg;
      _generation = new Chromosome[_alg.PopulationSize];
      Parallel.For(0, NumberOfChromosomes, i =>
      {
        _generation[i] = new Chromosome(_alg.NumberOfGenes, _alg.LengthOfGene, _seed);
      });
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
    }

    public void EvaluateFitnessOfPopulation()
    {
      Debug.Assert(_generation != null && _alg != null, "Is your constructor ok?");
      Parallel.ForEach(_generation, chromosome =>
      {
        double[] fitness = new double[_alg.NumberOfTrials];
        Parallel.For(0, _alg.NumberOfTrials, i =>
        {
          fitness[i] = _fitnessCalc(chromosome, this);
        });
        chromosome.Fitness = fitness.Average();
      });
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