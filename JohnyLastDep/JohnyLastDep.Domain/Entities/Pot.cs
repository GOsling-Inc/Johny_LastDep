using JohnyLastDep.Domain.Entities;

namespace JohnyLastDep.Domain.Models
{
	public class Pot
	{
		public int TotalAmount { get; set; } = 0;
		public List<Player> Participants { get; set; } = new List<Player>();

		public Pot()
		{
			TotalAmount = 0;
			Participants = new List<Player>();
		}

		public void AddBet(int betAmount, Player player)
		{
			if (betAmount <= 0)
			{
				throw new ArgumentException("Bet amount must be greater than zero.");
			}

			TotalAmount += betAmount;
			Console.WriteLine($"Общий банк: {TotalAmount}");
			if (!Participants.Contains(player))
			{
				Participants.Add(player);
			}
		}

		//public SidePot CreateSidePot(int sidePotAmount, List<Player> contributors)
		//{
		//	if (sidePotAmount <= 0)
		//	{
		//		throw new ArgumentException("Side pot amount must be greater than zero.");
		//	}

		//	var sidePot = new SidePot(sidePotAmount, contributors);
		//	SidePots.Add(sidePot);
		//	return sidePot;
		//}

		public void DistributeWinnings(Dictionary<Player, int> winnings)
		{
			foreach (var player in winnings)
			{
				player.Key.Chips += player.Value; 
			}
		}

		public int GetTotalAmount()
		{
			return TotalAmount;
		}

		public List<Player> GetParticipants()
		{
			return Participants;
		}

		//public List<SidePot> GetSidePots()
		//{
		//	return SidePots;
		//}
	}

	public class SidePot
	{
		public int Amount { get; private set; }
		public List<Player> Contributors { get; private set; }
		public SidePot() { }
		public SidePot(int amount, List<Player> contributors)
		{
			Amount = amount;
			Contributors = contributors;
		}
	}
}