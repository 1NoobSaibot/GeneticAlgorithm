namespace GeneticAlgorithmTest
{
	[TestClass]
	public class GeneticAlgorithmAsyncTest
	{
		[TestMethod]
		public async Task ShouldSortCorrectly()
		{
			for (int i = 0; i < 100; i++)
			{
				SimpleGAAsync gen = new();

				await gen.NextGeneration();

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
		public async Task ShouldProgress()
		{
			SimpleGAAsync gen = new();

			do
			{
				await gen.NextGeneration();
			} while (gen.GetChoosen()[0].Value != 777);
		}
	}
}
