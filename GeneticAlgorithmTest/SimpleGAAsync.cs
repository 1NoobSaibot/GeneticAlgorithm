using GeneticAlgorithm;

namespace GeneticAlgorithmTest
{
	/// <summary>
	/// This genetic algorithm should find the number 777 and all.
	/// Simple model, simple conditions, ideal test.
	/// </summary>
	internal class SimpleGAAsync : GeneticAlgorithm<SimpleModel>
	{
		public SimpleGAAsync() : base(new SimpleModelGenerator(), 100, 10) { }

		protected override ComparisonResult Compare(SimpleModel a, SimpleModel b)
		{
			if (a.Error < b.Error)
			{
				return ComparisonResult.A_IsGreater;
			}
			if (a.Error > b.Error)
			{
				return ComparisonResult.B_IsGreater;
			}
			return ComparisonResult.AreEqual;
		}

		protected override SimpleModel Cross(SimpleModel modelA, SimpleModel modelB)
		{
			return new SimpleModel((modelA.Value + modelB.Value) / 2f);
		}

		protected override SimpleModel Mutate(SimpleModel model)
		{
			return new SimpleModel(model.Value + (float)(Rand.NextDouble() * 20 - 10));
		}

		protected override Task TestCandidate(SimpleModel model)
		{
			model.Error = Math.Abs(777 - model.Value);
			return Task.CompletedTask;
		}
	}
}
