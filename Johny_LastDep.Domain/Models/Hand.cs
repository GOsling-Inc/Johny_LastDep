using System;
using System.Collections.Generic;
using System.Linq;
using Johny_LastDep.Domain.Enums;

namespace Johny_LastDep.Domain.Entities
{
	public class Hand
	{
		public List<Card> Cards { get; private set; }
		public HandType Type { get; private set; }
		public List<Rank> HighCards { get; private set; }

		public Hand(List<Card> cards)
		{
			if (cards == null || cards.Count != 5)
			{
				throw new ArgumentException("Hand must contain exactly 5 cards.");
			}
			Cards = new List<Card>(cards);
			HighCards = new List<Rank>();
			DetermineHandType();
		}

		private void DetermineHandType()
		{
			Cards = Cards.OrderByDescending(c => c.Rank).ToList();

			if (IsStraightFlush())
			{
				Type = HandType.StraightFlush;
				HighCards.Add(Cards[0].Rank);
			}
			else if (IsFourOfAKind())
			{
				Type = HandType.FourOfAKind;
				AddHighCardsForNOfAKind(4);
			}
			else if (IsFullHouse())
			{
				Type = HandType.FullHouse;
				AddHighCardsForFullHouse();
			}
			else if (IsFlush())
			{
				Type = HandType.Flush;
				HighCards.AddRange(Cards.Select(c => c.Rank));
			}
			else if (IsStraight())
			{
				Type = HandType.Straight;
				HighCards.Add(Cards[0].Rank);
			}
			else if (IsThreeOfAKind())
			{
				Type = HandType.ThreeOfAKind;
				AddHighCardsForNOfAKind(3);
			}
			else if (IsTwoPair())
			{
				Type = HandType.TwoPair;
				AddHighCardsForTwoPair();
			}
			else if (IsOnePair())
			{
				Type = HandType.OnePair;
				AddHighCardsForNOfAKind(2);
			}
			else
			{
				Type = HandType.HighCard;
				HighCards.AddRange(Cards.Select(c => c.Rank));
			}
		}

		private bool IsStraightFlush()
		{
			return IsFlush() && IsStraight();
		}

		private bool IsFourOfAKind()
		{
			return HasNOfAKind(4);
		}

		private bool IsFullHouse()
		{
			return HasNOfAKind(3) && HasNOfAKind(2);
		}

		private bool IsFlush()
		{
			return Cards.All(c => c.Suit == Cards[0].Suit);
		}

		private bool IsStraight()
		{
			for (int i = 0; i < Cards.Count - 1; i++)
			{
				if (Cards[i].Rank - 1 != Cards[i + 1].Rank)
				{
					return false;
				}
			}
			return true;
		}

		private bool IsThreeOfAKind()
		{
			return HasNOfAKind(3);
		}

		private bool IsTwoPair()
		{
			return Cards.GroupBy(c => c.Rank).Count(g => g.Count() == 2) == 2;
		}

		private bool IsOnePair()
		{
			return HasNOfAKind(2);
		}

		private bool HasNOfAKind(int n)
		{
			return Cards.GroupBy(c => c.Rank).Any(g => g.Count() == n);
		}

		private void AddHighCardsForNOfAKind(int n)
		{
			var groups = Cards.GroupBy(c => c.Rank)
							  .OrderByDescending(g => g.Count())
							  .ThenByDescending(g => g.Key)
							  .ToList();

			var mainGroup = groups.First(g => g.Count() == n);
			HighCards.Add(mainGroup.Key);

			var kickers = groups.Where(g => g.Count() != n)
								.Select(g => g.Key)
								.OrderByDescending(r => r)
								.ToList();

			HighCards.AddRange(kickers); 
		}

		private void AddHighCardsForFullHouse()
		{
			var groups = Cards.GroupBy(c => c.Rank)
							  .OrderByDescending(g => g.Count())
							  .ThenByDescending(g => g.Key)
							  .ToList();

			var threeOfAKindGroup = groups.First(g => g.Count() == 3);
			HighCards.Add(threeOfAKindGroup.Key);

			var pairGroup = groups.First(g => g.Count() == 2);
			HighCards.Add(pairGroup.Key);
		}


		private void AddHighCardsForTwoPair()
		{
			var groups = Cards.GroupBy(c => c.Rank)
							  .Where(g => g.Count() == 2)
							  .OrderByDescending(g => g.Key)
							  .ToList();

			HighCards.AddRange(groups.Select(g => g.Key));

			var kicker = Cards.First(c => !HighCards.Contains(c.Rank)).Rank;
			HighCards.Add(kicker);
		}

		public static int CompareHands(Hand hand1, Hand hand2)
		{
			if (hand1.Type != hand2.Type)
			{
				return hand1.Type.CompareTo(hand2.Type);
			}

			for (int i = 0; i < Math.Min(hand1.HighCards.Count, hand2.HighCards.Count); i++)
			{
				int comparison = hand1.HighCards[i].CompareTo(hand2.HighCards[i]);
				if (comparison != 0)
				{
					return comparison; 
				}
			}

			return 0;
		}
	}
}
