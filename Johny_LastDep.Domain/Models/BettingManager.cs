using System;
using System.Collections.Generic;
using Johny_LastDep.Domain.Entities;

namespace Johny_LastDep.Domain.Models
{
	public class BettingManager
	{
		public int CurrentBet { get; private set; }
		public int Pot { get; private set; }
		public List<Player> Players { get; private set; }

		public BettingManager(List<Player> players)
		{
			Players = players;
			Pot = 0;
			CurrentBet = 0;
		}

		public void PlaceBet(Player player, int amount)
		{
			if (amount < CurrentBet)
			{
				throw new ArgumentException("Ставка должна быть больше или равна текущей ставке.");
			}

			if (player.Chips < amount)
			{
				throw new InvalidOperationException("Недостаточно фишек для ставки.");
			}

			player.Chips -= amount;
			Pot += amount;
			CurrentBet = amount;

			Console.WriteLine($"{player.Name} ставит {amount}. Текущий банк: {Pot}");
		}

		public void HandleAllIn(Player player)
		{
			int allInAmount = player.Chips;
			PlaceBet(player, allInAmount);
			Console.WriteLine($"{player.Name} идет ол-ин на {allInAmount}.");
		}

		public void ResetBets()
		{
			foreach (var player in Players)
			{
				player.CurrentBet = 0; 
			}
			Pot = 0; 
			CurrentBet = 0; 
		}
	}
}