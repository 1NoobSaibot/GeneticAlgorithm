using Evolution;

namespace GeneticAlgorithmTest
{
	[TestClass]
	public class GeneticAlgorithmTest
	{
		[TestMethod]
		public void ShouldSortCorrectly()
		{
			for (int i = 0; i < 100; i++)
			{
				SimpleGA gen = new SimpleGA();
				gen.LoadCandidate(new SimpleModel(-777));
				gen.LoadCandidate(new SimpleModel(-4));

				gen.NextGeneration();
				gen.NextGeneration();

				var models = gen.GetChoosen();
				for (int j = 1; j < models.Length; j++)
				{
					var aDist = Math.Abs(models[j - 1].Value - 777);
					var bDist = Math.Abs(models[j - 0].Value - 777);
					Assert.IsTrue(aDist <= bDist);
				}
				
			}
		}


		[TestMethod]
		public void ShouldProgress()
		{
			SimpleGA gen = new SimpleGA();
			gen.LoadCandidate(new SimpleModel(-777));
			gen.LoadCandidate(new SimpleModel(-4));

			do
			{
				gen.NextGeneration();
			} while (gen.GetChoosen()[0].Value != 777);
		}
	}


	/// <summary>
	/// This genetic algorithm should find the number 777 and all.
	/// Simple model, simple conditions, ideal test.
	/// </summary>
	internal class SimpleGA : GeneticAlgorithm<SimpleModel>
	{
		public SimpleGA():base(100, 10) { }

		public override ComparisonResult Compare(SimpleModel a, SimpleModel b)
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

		public override SimpleModel Cross(SimpleModel modelA, SimpleModel modelB)
		{
			return new SimpleModel((modelA.Value + modelB.Value) / 2f);
		}

		public override SimpleModel Mutate(SimpleModel model)
		{
			return new SimpleModel(model.Value + (float)(Rand.NextDouble() * 20 - 10));
		}

		public override void TestCandidate(SimpleModel model)
		{
			model.Error = Math.Abs(777 - model.Value);
		}
	}


	internal class SimpleModel
	{
		public readonly float Value;
		public float Error;

		public SimpleModel(float value)
		{
			Value = value;
		}


		public override string ToString()
		{
			return Math.Abs(Value - 777).ToString();
		}
	}
}
