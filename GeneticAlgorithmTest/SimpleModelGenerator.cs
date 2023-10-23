using Evolution;

namespace GeneticAlgorithmTest
{
	internal class SimpleModelGenerator : IModelGenerator<SimpleModel>
	{
		private readonly Random rnd = new();

		public SimpleModel Generate()
		{
			return new SimpleModel(rnd.NextSingle() * 100 - 700);
		}
	}
}
