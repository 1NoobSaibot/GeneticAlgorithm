namespace Evolution
{
	public interface IGeneticAlgorithm<Model> where Model : IGeneticModel
	{
		// Before looping generations
		void LoadCandidate(Model model);

		/// <summary>
		/// The way to create a new candidate from old one.
		/// </summary>
		/// <param name="model">Parent</param>
		/// <returns></returns>
		Model Mutate(Model model);


		/// <summary>
		/// The way to make a new candidate from two old candidates;
		/// By default it is just a random mutation of modelA or modelB
		/// </summary>
		/// <param name="modelA">First parent</param>
		/// <param name="modelB">Second parent</param>
		/// <returns></returns>
		Model Cross(Model modelA, Model modelB);

		/// <summary>
		/// Fitnes function.
		/// </summary>
		/// <param name="model"></param>
		void TestCandidate(Model model);

		ComparisonResult Compare(Model modelA, Model modelB);

		/// <summary>
		/// Returns the best model at the latest generation
		/// </summary>
		/// <returns></returns>
		Model[] GetChoosen();
	}
}
