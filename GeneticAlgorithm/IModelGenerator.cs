namespace Evolution
{
	public interface IModelGenerator<Model> where Model : IGeneticModel
	{
		Model Generate();
	}
}
