using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GeneticAlgorithmTest
{
	[TestClass]
	public class GeneticAlgorithmTest
	{
		[TestMethod]
		public void TestMethod1()
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
		public SimpleGA():base(100, 2) { }

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
	}
}
