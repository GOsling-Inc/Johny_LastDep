using System;
using System.Collections.Generic;
using Johny_LastDep.Domain.Entities;

namespace Johny_LastDep.Domain.Models
{
	public class GameState
	{
		public int CurrentRound { get; private set; }
		public List<Player> ActivePlayers { get; private set; }
		public int Pot { get; set; }
		public List<string> ActionHistory { get; private set; }

		public GameState(List<Player> players)
		{
			CurrentRound = 0;
			ActivePlayers = new List<Player>(players);
			Pot = 0;
			ActionHistory = new List<string>();
		}

		public void IncrementRound()
		{
			CurrentRound++;
			Console.WriteLine($"Начался раунд {CurrentRound}.");
		}

		public void UpdatePot(int amount)
		{
			Pot += amount;
			Console.WriteLine($"Размер банка увеличился на {amount}. Текущий банк: {Pot}");
		}

		public void AddAction(string action)
		{
			ActionHistory.Add(action);
			Console.WriteLine($"Действие добавлено в историю: {action}");
		}

		public void RemovePlayer(Player player)
		{
			ActivePlayers.Remove(player);
			Console.WriteLine($"{player.Name} выбыл из игры.");
		}

		public void ResetGameState(List<Player> players)
		{
			CurrentRound = 0;
			ActivePlayers = new List<Player>(players);
			Pot = 0;
			ActionHistory.Clear();
			Console.WriteLine("Состояние игры сброшено.");
		}
	}
}