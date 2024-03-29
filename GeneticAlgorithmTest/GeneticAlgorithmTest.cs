﻿namespace GeneticAlgorithmTest
{
	[TestClass]
	public class GeneticAlgorithmTest
	{
		[TestMethod]
		public void ShouldSortCorrectly()
		{
			for (int i = 0; i < 100; i++)
			{
				SimpleGA gen = new();

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
			SimpleGA gen = new();

			do
			{
				gen.NextGeneration();
			} while (gen.GetChoosen()[0].Value != 777);
		}
	}
}
