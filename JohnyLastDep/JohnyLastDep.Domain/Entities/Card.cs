using JohnyLastDep.Domain.Enums;

namespace JohnyLastDep.Domain.Entities
{
	public class Card
	{
		public Suit Suit { get; set; }
		public Rank Rank { get; set; }

		public Card(Suit suit, Rank rank)
		{
			Suit = suit;
			Rank = rank;
		}

		public override string ToString()
		{
			string[] suits = { "♣", "♦", "♥", "♠" };
			string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
			return $"{ranks[(int)Rank - 2]}{suits[(int)Suit]}";
		}

		public static bool operator >(Card card1, Card card2)
		{
			return card1.Rank > card2.Rank;
		}

		public static bool operator <(Card card1, Card card2)
		{
			return card1.Rank < card2.Rank;
		}

		public static bool operator >=(Card card1, Card card2)
		{
			return card1.Rank >= card2.Rank;
		}

		public static bool operator <=(Card card1, Card card2)
		{
			return card1.Rank <= card2.Rank;
		}
	}
}
