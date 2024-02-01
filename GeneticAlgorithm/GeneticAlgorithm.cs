namespace Evolution
{
	public abstract class GeneticAlgorithm<Model> : IGeneticAlgorithm<Model>
		where Model : class, IGeneticModel
	{
		private readonly IModelGenerator<Model> _generator;
		public readonly int GenerationLength;
		public readonly int AmountOfChoosen;
		public int GenerationCounter { get; private set; }
		public bool IsEmpty => _candidates[0] == null;


		protected readonly Random Rand = new();
		private readonly Model?[] _candidates;


		public GeneticAlgorithm(
			IModelGenerator<Model> generator,
			int generationLength,
			int amountOfChoosen
		)
		{
			if (generationLength < 2)
			{
				throw new ArgumentException("GenerationLength cannot be less than 2");
			}

			if (amountOfChoosen >= generationLength)
			{
				throw new ArgumentOutOfRangeException(nameof(amountOfChoosen), $"should be less than {nameof(generationLength)}");
			}

			_generator = generator;
			GenerationLength = generationLength;
			AmountOfChoosen = amountOfChoosen;
			_candidates = new Model[generationLength];
		}


		public GeneticAlgorithm(
			IModelGenerator<Model> generator,
			int generationLength,
			int amountOfChoosen,
			int generationCountInit
		)
			: this(generator, generationLength, amountOfChoosen)
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
			do
			{
				ReproduceAndMutate();
				TestCandidates(_candidates);
				ChooseTheBest();
			} while (_candidates[0] == null);

			AfterSelection();
			GenerationCounter++;
		}


		public Model[] GetChoosen()
		{
			List<Model> choosenCandidates = new();
			for (int i = 0; i < AmountOfChoosen; i++)
			{
				if (_candidates[i] != null)
				{
					choosenCandidates.Add(_candidates[i]!);
				}
			}
			return choosenCandidates.ToArray();
		}


		public void InitGenerationCounter(int value)
		{
			if (GenerationCounter != 0)
			{
				throw new Exception("GenerationCounter was inited before");
			}

			if (value < 0)
			{
				throw new ArgumentException("Cannot set negative value to GenerationCounter");
			}

			GenerationCounter = value;
		}


		/// <summary>
		/// This method remove all models to generate new generation from zero.
		/// <see cref="GenerationCounter"/> will not be reset
		/// </summary>
		public void ResetModels()
		{
			for (int i = 0; i < _candidates.Length; i++)
			{
				_candidates[i] = null;
			}
		}


		private void ReproduceAndMutate()
		{
			if (_candidates[0] == null)
			{
				for (int i = 0; i < _candidates.Length; i++)
				{
					_candidates[i] = _generator.Generate();
				}
				return;
			}

			if (_candidates[1] == null)
			{
				_candidates[1] = Mutate(_candidates[0]!);
			}

			for (int i = 2; i < _candidates.Length; i++)
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
		protected virtual void TestCandidates(Model?[] candidates)
		{
			for (int i = 0; i < candidates.Length; i++)
			{
				TestCandidate(candidates[i]!);
			}
		}


		private void ChooseTheBest()
		{
			Model?[] choosen = new Model[AmountOfChoosen];
			for (int j = 0; j < _candidates.Length; j++)
			{
				Model candidate = _candidates[j]!;
				_candidates[j] = null;

				if (candidate!.CanBeTested() == false)
				{
					continue;
				}

				for (int i = 0; i < choosen.Length; i++)
				{
					if (choosen[i] == null)
					{
						choosen[i] = candidate;
						break;
					}

					var comRes = Compare(candidate, choosen[i]!);
					if (comRes == ComparisonResult.A_IsGreater || comRes == ComparisonResult.AreEqual)
					{
						(candidate, choosen[i]) = (choosen[i]!, candidate);
					}
				}
			}

			if (choosen[0] == null)
			{
				return;
			}

			for (int i = 0; i < choosen.Length; i++)
			{
				if (choosen[i] == null)
				{
					break;
				}
				_candidates[i] = choosen[i];
			}
		}


		private Model ChooseRandomCandidate(int maxIndex)
		{
			Model? c;
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
					return _candidates[i]!;
				}
			}

			throw new Exception("Cannot choose random candidate to mutate: There is an empty array of candidates");
		}


		public abstract ComparisonResult Compare(Model a, Model b);
		public abstract Model Mutate(Model model);


		public virtual Model Cross(Model modelA, Model modelB)
		{
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
		public virtual void TestCandidate(Model model)
		{
			// Do nothing
		}


		/// <summary>
		/// This hook will be called after new generation is sorted.
		/// But before GenerationCounter is updated!
		/// </summary>
		protected virtual void AfterSelection()
		{ }
	}


	public enum ComparisonResult
	{
		A_IsGreater = -1,
		B_IsGreater = 1,
		AreEqual = 0
	}
}