namespace GeneticAlgorithm
{
	public interface IGeneticAlgorithm
	{
		Task NextGeneration();
	}


	public interface IGeneticAlgorithm<Model> : IGeneticAlgorithm
		where Model : IGeneticModel
	{
		void LoadCandidate(Model model);
		Model[] GetChoosen();
	}
}
