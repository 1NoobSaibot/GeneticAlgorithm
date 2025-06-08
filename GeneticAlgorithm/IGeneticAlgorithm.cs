namespace GeneticAlgorithm
{
	public interface IGeneticAlgorithmAsync
	{
		Task NextGeneration();
	}


	public interface IGeneticAlgorithm<Model> : IGeneticAlgorithmAsync
		where Model : IGeneticModel
	{
		void LoadCandidate(Model model);
		Model[] GetChoosen();
	}
}
