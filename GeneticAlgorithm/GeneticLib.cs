namespace GeneticAlgorithm
{
    public static class GeneticLib 
    {
        public static IGeneticAlgorithm CreateGeneticAlgorithm(int populationSize, int numberOfGenes, int lengthOfGene, double mutationRate, double eliteRate, int numberOfTrials, FitnessEventHandler fitnessCalculation, int? seed = null)
        {
            //To be removed
            throw new System.NotImplementedException("Method not implemented");
            //return new GeneticAlgorithm(populationSize, numberOfGenes, lengthOfGene, mutationRate, eliteRate, numberOfTrials, fitnessCalculation, seed);
        }
    }
}