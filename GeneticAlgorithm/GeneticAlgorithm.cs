using System;


public abstract class GeneticAlgorithm<Model> : IGeneticAlgorithm<Model> where Model : class
{
	public readonly int GenerationLength;
	public readonly int AmountOfChoosen;
	public int GenerationCounter { get; private set; }


	protected readonly Random Rand = new Random();
	private Model[] _candidates;


	public GeneticAlgorithm(int generationLength, int amountOfChoosen)
	{
		if (amountOfChoosen >= generationLength)
		{
			throw new ArgumentOutOfRangeException("Amount of Choosen candidates should be less than Generation Length");
		}
		GenerationLength = generationLength;
		AmountOfChoosen = amountOfChoosen;
		_candidates = new Model[generationLength];
	}


	public void LoadCandidate(Model model)
	{
		for (int i = 0; i < _candidates.Length; i++)
		{
			if (_candidates[i] == null)
			{
				_candidates[i] = model;
				return;
			}
		}

		throw new Exception("There is no place to push new model");
	}


	public void NextGeneration()
	{
		ReproduceAndMutate();
		TestCandidates();
		MakeGenocide();
		GenerationCounter++;
	}


	public Model[] GetChoosen()
	{
		Model[] choosenCandidates = new Model[AmountOfChoosen];
		for (int i = 0; i < choosenCandidates.Length; i++)
		{
			choosenCandidates[i] = _candidates[i];
		}
		return choosenCandidates;
	}


	private void ReproduceAndMutate()
	{
		for (int i = 0; i < _candidates.Length; i++)
		{
			if (i < AmountOfChoosen && _candidates[i] != null)
			{
				continue;
			}

			Model candidateA = ChooseRandomCandidate(i);
			if (Rand.NextDouble() < 0.5)
			{
				_candidates[i] = Mutate(candidateA);
			}
			else
			{
				Model candidateB = ChooseRandomCandidate(i);

				_candidates[i] = Cross(candidateA, candidateB);
			}
		}
	}


	private void TestCandidates()
	{
		for (int i = 0; i < _candidates.Length; i++)
		{
			TestCandidate(_candidates[i]);
		}
	}


	private void MakeGenocide()
	{
		Array.Sort(_candidates, Compare);
	}


	private Model ChooseRandomCandidate(int maxIndex)
	{
		Model c;
		int loopCounter = 0;
		do
		{
			int index = Rand.Next(maxIndex);
			c = _candidates[index];

			if (c != null)
			{
				return c;
			}

			loopCounter++;
		} while (c != null && loopCounter < 1000);

		for (int i = 0; i < _candidates.Length; i++)
		{
			if (_candidates[i] != null)
			{
				return _candidates[i];
			}
		}

		throw new Exception("Cannot choose random candidate to mutate: There is an empty array of candidates");
	}


	public abstract int Compare(Model a, Model b);
	public abstract Model Mutate(Model model);
	public abstract Model Cross(Model modelA, Model modelB);
	public abstract void TestCandidate(Model model);
}
