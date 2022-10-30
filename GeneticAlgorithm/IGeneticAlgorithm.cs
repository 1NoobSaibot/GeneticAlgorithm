public interface IGeneticAlgorithm<Model>
{
	// Before looping generations
	void LoadCandidate(Model model);

	// Reproduce and Mutate
	Model Mutate(Model model);
	Model Cross(Model modelA, Model modelB);

	/// <summary>
	/// Fitnes function.
	/// </summary>
	/// <param name="model"></param>
	void TestCandidate(Model model);

	/// <summary>
	/// Genocide
	/// </summary>
	/// <param name="modelA"></param>
	/// <param name="modelB"></param>
	/// <returns></returns>
	int Compare(Model modelA, Model modelB);

	/// <summary>
	/// Returns the best model at the latest generation
	/// </summary>
	/// <returns></returns>
	Model[] GetChoosen();
}
