using System;

namespace GeneticAlgorithm
{
  internal class Generation : IGenerationDetails
  {
    private IGeneticAlgorithm _alg;
    private FitnessEventHandler _fitnessCalc;
    private int? _seed;
    private Chromosome[] _generation;
    public IChromosome this[int index] => throw new System.NotImplementedException();

    public double AverageFitness => throw new System.NotImplementedException();

    public double MaxFitness => throw new System.NotImplementedException();

    public long NumberOfChromosomes {get;}

    internal Generation(IGeneticAlgorithm alg, FitnessEventHandler fitnessCalc, int? seed)
    {
      _fitnessCalc = fitnessCalc;
      _seed = seed;
      _alg = alg;
      NumberOfChromosomes = _alg.PopulationSize;
      _generation = new Chromosome[NumberOfChromosomes];
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
      NumberOfChromosomes = chromosomes.Length;
      _generation = new Chromosome[NumberOfChromosomes];
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
      throw new System.NotImplementedException();
    }

    public IChromosome SelectParent()
    {
      throw new System.NotImplementedException();
    }
  }
}