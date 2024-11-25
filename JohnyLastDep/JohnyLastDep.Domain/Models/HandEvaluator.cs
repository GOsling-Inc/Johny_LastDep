using JohnyLastDep.Domain.Entities;

namespace JohnyLastDep.Domain.Models
{
	public class HandEvaluator
	{
		public Hand EvaluateHand(List<Card> playerCards, List<Card> communityCards)
		{
			var allCards = new List<Card>(playerCards);
			allCards.AddRange(communityCards);

			if (allCards.Count < 5)
			{
				throw new ArgumentException("Not enough cards to evaluate hand.");
			}

			var combinations = GetCombinations(allCards, 5);
			Hand bestHand = null;

			foreach (var combination in combinations)
			{
				var hand = new Hand(combination);
				if (bestHand == null || Hand.CompareHands(hand, bestHand) > 0)
				{
					bestHand = hand;
				}
			}

			return bestHand;
		}

        private IEnumerable<List<Card>> GetCombinations(List<Card> cards, int combinationSize)
		{
			if (combinationSize > cards.Count)
			{
				yield break;
			}

			var indices = Enumerable.Range(0, combinationSize).ToArray();
			yield return indices.Select(i => cards[i]).ToList();

			while (true)
			{
				int i;
				for (i = combinationSize - 1; i >= 0 && indices[i] == cards.Count - combinationSize + i; i--) ;
				if (i < 0) yield break;

				indices[i]++;
				for (int j = i + 1; j < combinationSize; j++)
				{
					indices[j] = indices[j - 1] + 1;
				}

				yield return indices.Select(index => cards[index]).ToList();
			}
		}

		public int CompareHands(Hand hand1, Hand hand2)
		{
			return Hand.CompareHands(hand1, hand2);
		}
	}
}