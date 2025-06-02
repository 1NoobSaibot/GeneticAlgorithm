using GeneticAlgorithm;

namespace GeneticAlgorithmTest
{
	internal class SimpleModel : IGeneticModel
	{
		public readonly float Value;
		public float Error;

		public SimpleModel(float value)
		{
			Value = value;
		}

		public bool CanBeTested()
		{
			return true;
		}

		public override string ToString()
		{
			return Math.Abs(Value - 777).ToString();
		}
	}
}
