using JohnyLastDep.Domain.Entities;

namespace JohnyLastDep.Domain.Models
{
	public class BettingManager
	{
		public int CurrentBet { get; private set; }
		public Pot _pot { get; private set; }
		public List<Player> Players { get; private set; }

		public BettingManager(List<Player> players, Pot pot)
		{
			Players = players;
			_pot = pot;
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

			player.Call(amount);
			_pot.AddBet(amount, player);
			CurrentBet = amount;
			Console.WriteLine($"{player.Name} ставит {amount}. Текущий банк: {_pot.TotalAmount}");
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