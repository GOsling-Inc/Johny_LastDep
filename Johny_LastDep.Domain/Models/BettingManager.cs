using System;
using System.Collections.Generic;
using Johny_LastDep.Domain.Entities;

namespace Johny_LastDep.Domain.Models
{
	public class BettingManager
	{
		public int CurrentBet { get; private set; }
		public Pot _pot { get; private set; }
		public List<Player> Players { get; private set; }

		public BettingManager(List<Player> players)
		{
			Players = players;
			_pot = new Pot();
			CurrentBet = 0;
		}

		public void Next() {
			CurrentBet = 0;
		}

		public void PlaceBet(Player player, int amount)
		{
			if (player.CurrentBet + amount < CurrentBet)
			{
				throw new ArgumentException("Ставка должна быть больше или равна текущей ставке.");
			}

			if (player.Chips < amount)
			{
				throw new InvalidOperationException("Недостаточно фишек для ставки.");
			}

			player.Bet(amount);
			_pot.AddBet(amount, player);
			CurrentBet = player.CurrentBet;
			Console.WriteLine($"{player.Name} ставит {amount}. Текущий ставка: {CurrentBet}");
		}

		public void Check(Player player) {
			if (player.CurrentBet < CurrentBet) {
				throw new ArgumentException("Повысьте ставку.");
			}
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
			_pot = new Pot(); 
			CurrentBet = 0; 
		}
	}
}