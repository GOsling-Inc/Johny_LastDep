using JohnyLastDep.Domain.Entities;

namespace JohnyLastDep.Domain.Models
{
	public class BettingManager
	{
		public int CurrentBet { get; set; } = 0;
		public Pot Pot { get; set; } = new Pot();
		public List<Player> Players { get; set; }
		public BettingManager() { }
		public BettingManager(List<Player> players)
		{
			Players = players;
			Pot = new Pot();
			CurrentBet = 0;
		}
		public void Next()
		{
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
			Pot.AddBet(amount, player);
			CurrentBet = player.CurrentBet;
			Console.WriteLine($"{player.Name} ставит {amount}. Текущий ставка: {CurrentBet}");
		}
		public void Check(Player player)
		{
			if (player.CurrentBet < CurrentBet)
			{
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
			Pot = new Pot();
			CurrentBet = 0;
		}
	}
}