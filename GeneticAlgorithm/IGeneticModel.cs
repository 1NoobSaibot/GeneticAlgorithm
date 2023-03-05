namespace Evolution
{
	public interface IGeneticModel
	{
		/// <summary>
		/// Return false if measurements cannot be processed in fitness function.
		/// Container will remove the model.
		/// </summary>
		/// <returns></returns>
		bool CanBeTested();
	}
}
