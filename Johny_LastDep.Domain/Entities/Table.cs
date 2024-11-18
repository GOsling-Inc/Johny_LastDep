using System;
using System.Collections.Generic;
using Johny_LastDep.Domain.Entities;

namespace Johny_LastDep.Domain.Models
{
	public class Table
	{
		public List<Player> Players { get; private set; }
		public int DealerPosition { get; private set; }
		public int SmallBlind { get; private set; }
		public int BigBlind { get; private set; }
		public int MinimumBet { get; private set; }
		public int MaximumBet { get; private set; }
		public List<Card> CommunityCards { get; private set; }

		public Table(List<Player> players, int dealerPosition, int smallBlind, int bigBlind, int minimumBet, int maximumBet)
		{
			Players = players;
			DealerPosition = dealerPosition;
			SmallBlind = smallBlind;
			BigBlind = bigBlind;
			MinimumBet = minimumBet;
			MaximumBet = maximumBet;
			CommunityCards = new List<Card>();
		}

		public void AddCommunityCard(Card card)
		{
			if (CommunityCards.Count < 5) 
			{
				CommunityCards.Add(card);
			}
			else
			{
				throw new InvalidOperationException("Нельзя добавлять больше 5 общих карт.");
			}
		}

		public void ResetCommunityCards()
		{
			CommunityCards.Clear();
		}

		public void MoveDealerPosition()
		{
			DealerPosition = (DealerPosition + 1) % Players.Count;
		}

		public void DisplayCommunityCards()
		{
			Console.WriteLine($"Общие карты: {string.Join(", ", CommunityCards)}");
		}
	}
}