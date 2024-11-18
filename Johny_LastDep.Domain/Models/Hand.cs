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
				HighCards.Add(Cards.GroupBy(c => c.Rank)
									.OrderByDescending(g => g.Key)
									.First().Key); 
			}
			else if (IsFullHouse())
			{
				Type = HandType.FullHouse;
				var groups = Cards.GroupBy(c => c.Rank).OrderByDescending(g => g.Key).ToList();
				HighCards.Add(groups[0].Key); 
				HighCards.Add(groups[1].Key); 
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
				HighCards.Add(Cards.GroupBy(c => c.Rank)
									.OrderByDescending(g => g.Key)
									.First().Key); 
				HighCards.AddRange(Cards.Select(c => c.Rank)
										.Distinct()
										.Where(r => r != HighCards[0])
										.OrderByDescending(r => r)
										.Take(2));
			}
			else if (IsTwoPair())
			{
				Type = HandType.TwoPair;
				var pairs = Cards.GroupBy(c => c.Rank)
								 .Where(g => g.Count() == 2)
								 .OrderByDescending(g => g.Key)
								 .ToList();
				HighCards.AddRange(pairs.Select(p => p.Key)); 
				HighCards.Add(Cards.First(c => !HighCards.Contains(c.Rank)).Rank); 
			}
			else if (IsOnePair())
			{
				Type = HandType.OnePair;
				HighCards.Add(Cards.GroupBy(c => c.Rank)
									.OrderByDescending(g => g.Key)
									.First().Key); 
				HighCards.AddRange(Cards.Select(c => c.Rank)
										.Distinct()
										.Where(r => r != HighCards[0])
										.OrderByDescending(r => r)
										.Take(3)); 
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
			var pairs = Cards.GroupBy(c => c.Rank).Where(g => g.Count() == 2).Count();
			return pairs == 2;
		}

		private bool IsOnePair()
		{
			return HasNOfAKind(2); 
		}

		private bool HasNOfAKind(int n)
		{
			return Cards.GroupBy(c => c.Rank).Any(g => g.Count() == n);
		}

		public static int CompareHands(Hand hand1, Hand hand2)
		{
			if (hand1.Type != hand2.Type)
			{
				return hand1.Type.CompareTo(hand2.Type);
			}
			else
			{
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
}