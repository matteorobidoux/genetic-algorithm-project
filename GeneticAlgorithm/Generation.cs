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
        if(index <= 0 || index >= NumberOfChromosomes)
        {
          throw new IndexOutOfRangeException($"Invalid index for Generation. Expected between 0 and {NumberOfChromosomes}. Got: {index}");
        }
        return _generation[index];
      }
    }

    public double AverageFitness {get; private set;}

    public double MaxFitness {get; private set;}
    public long NumberOfChromosomes {get => _generation.Length;}

    internal Generation(IGeneticAlgorithm alg, FitnessEventHandler fitnessCalc, int? seed)
    {
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
      foreach (Chromosome chromosome in _generation)
      {
        double fitness = _fitnessCalc(chromosome, this);
        chromosome.Fitness = fitness;
        totalFitness += fitness;
        if (fitness > MaxFitness)
        {
          MaxFitness = fitness;
        }
      }
      AverageFitness = totalFitness/NumberOfChromosomes;
      Array.Sort(_generation);
    }

    public IChromosome SelectParent()
    {
      throw new System.NotImplementedException();
    }
  }
}