﻿namespace JohnyLastDep.Domain.Entities
{
	public class Player
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Card> HandCards { get; set; }
		public int Chips { get; set; }
		public int CurrentBet { get; set; }
		public bool IsPlaying { get; set; }

		public Player(string name, int startingChips)
		{
			Id = Guid.NewGuid().ToString("N");
			Name = name;
			Chips = startingChips;
			HandCards = [];
			CurrentBet = 0;
			IsPlaying = true;
		}

		public void Fold()
		{
			IsPlaying = false;
		}

		public void Bet(int amount)
		{
			Chips -= amount;
			CurrentBet += amount;
		}

		public void ResetHand()
		{
			HandCards.Clear();
			CurrentBet = 0;
			IsPlaying = true;
		}
	}
}
