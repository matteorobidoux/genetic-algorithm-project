using System;

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
        if(index < 0 || index >= NumberOfChromosomes)
        {
          throw new IndexOutOfRangeException($"Invalid index for Generation. Expected between 0 and {NumberOfChromosomes}. Got: {index}");
        }
        return _generation[index];
      }
    }

    public double AverageFitness {get; private set;}

    public double MaxFitness {get; private set;}
    public long NumberOfChromosomes {get => _generation.Length;}

    internal Generation(IGeneticAlgorithm alg, FitnessEventHandler fitnessCalc, int? seed = null)
    {
      if (alg == null)
      {
        throw new ArgumentNullException("The algorithm that the generation uses cannot be null");
      }
      if (fitnessCalc == null)
      {
        throw new ArgumentNullException($"The fitness calculation delegate cannot be null");
      }
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
      //TODO: Add a check to see if the input chromosomes follow the specifications of the generation
      if (chromosomes == null)
      {
        throw new ArgumentNullException($"The chromosomes of the generation cannot be null");
      }
      if (generation == null)
      {
        throw new ArgumentNullException($"The generation cannot be null");
      }
      _fitnessCalc = generation._fitnessCalc;
      _seed = generation._seed;
      _alg = generation._alg;
      _generation = new Chromosome[chromosomes.Length];
      for (int i = 0; i < NumberOfChromosomes; i++)
      {
        _generation[i] = new Chromosome(chromosomes[i].Genes, _alg.LengthOfGene, _seed);
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

    public void EvaluateFitnessOfPopulation()
    {
      double totalFitness = 0;
      for (int i = 0; i < _alg.NumberOfTrials; i++)
      {
        foreach (Chromosome chromosome in _generation)
        {
          double fitness = _fitnessCalc(chromosome, this) / _alg.NumberOfTrials;
          chromosome.Fitness += fitness;
          totalFitness += fitness;
          if (chromosome.Fitness > MaxFitness)
          {
            MaxFitness = chromosome.Fitness;
          }
        }
      }
      AverageFitness = totalFitness/NumberOfChromosomes;
      Array.Sort(_generation);
    }

    public IChromosome SelectParent()
    {
      Random rand = GetRandomObj();
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
  }
}