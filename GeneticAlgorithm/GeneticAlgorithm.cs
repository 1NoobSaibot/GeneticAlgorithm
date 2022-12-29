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


	public GeneticAlgorithm(int generationLength, int amountOfChoosen, int generationCountInit)
		: this(generationLength, amountOfChoosen)
	{
		GenerationCounter = generationCountInit;
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
		TestCandidates(_candidates);
		MakeGenocide();
		AfterGenocide();
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


	/// <summary>
	/// By default tests each candidate separately by calling TestCandidate(Model).
	/// If you need to test models in different way you can override the method.
	/// </summary>
	protected virtual void TestCandidates(Model[] candidates)
	{
		for (int i = 0; i < candidates.Length; i++)
		{
			TestCandidate(candidates[i]);
		}
	}


	private void MakeGenocide()
	{
		Model[] choosen = new Model[AmountOfChoosen];
		foreach (var candidate in _candidates)
		{
			Model tryInsert = candidate;
			for (int i = 0; i < choosen.Length;i++)
			{
				if (choosen[i] == null)
				{
					choosen[i] = tryInsert;
					break;
				}

				var comRes = Compare(tryInsert, choosen[i]);
				if (comRes == ComparisonResult.A_IsGreater || comRes == ComparisonResult.AreEqual)
				{
					Model temp = choosen[i];
					choosen[i] = tryInsert;
					tryInsert = temp;
				}
			}
		}

		for (int i = 0; i < _candidates.Length; i++)
		{
			_candidates[i] = i < choosen.Length ? choosen[i] : null;
		}
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


	public abstract ComparisonResult Compare(Model a, Model b);
	public abstract Model Mutate(Model model);


	public virtual Model Cross(Model modelA, Model modelB) {
		if (Rand.Next() % 2 == 0)
		{
			return Mutate(modelA);
		}
		return Mutate(modelB);
	}


	/// <summary>
	/// By default this method will be called by TestCandidates() method to test each model separately.
	/// You don't have to implement the method
	/// </summary>
	/// <param name="model">AI Model, Candidate</param>
	public virtual void TestCandidate(Model model) {
		// Do nothing
	}


	/// <summary>
	/// This hook will be called after new generation is sorted.
	/// But before GenerationCounter is updated!
	/// </summary>
	protected virtual void AfterGenocide()
	{ }
}


public enum ComparisonResult
{
	A_IsGreater = -1,
	B_IsGreater = 1,
	AreEqual = 0
}
