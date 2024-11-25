using JohnyLastDep.Domain.Entities;
using JohnyLastDep.Domain.Enums;

namespace JohnyLastDep.Domain.Models
{
	public class Deck
	{
		private List<Card> cards;
		private Random random;

		public Deck()
		{
			cards = new List<Card>();
			random = new Random();
			Reset();
		}

		public void Reset()
		{
			cards.Clear();
			foreach (Suit suit in Enum.GetValues(typeof(Suit)))
			{
				foreach (Rank rank in Enum.GetValues(typeof(Rank)))
				{
					cards.Add(new Card(suit, rank));
				}
			}
		}

		public void Shuffle()
		{
			int n = cards.Count;
			while (n > 1)
			{
				n--;
				int k = random.Next(n + 1);
				Card temp = cards[k];
				cards[k] = cards[n];
				cards[n] = temp;
			}
		}

		public Card Deal()
		{
			if (cards.Count == 0)
			{
				throw new InvalidOperationException("Empty deck");
			}
			Card card = cards[0];
			cards.RemoveAt(0);
			return card;
		}
	}
}
