using JohnyLastDep.Domain.Entities;

namespace JohnyLastDep.Domain.Models
{
	public class Table
	{
		public List<Player> Players { get; set; } = new List<Player>();
		public int DealerPosition { get; set; } = 0;
		public List<Card> CommunityCards { get; set; } = new List<Card>();

		public Table() { }
		public Table(List<Player> players, int dealerPosition)
		{
			Players = players;
			DealerPosition = dealerPosition;
			CommunityCards = new List<Card>();
		}

		public void AddCommunityCard(Card card)
		{
			if (CommunityCards.Count < 5)
			{
				CommunityCards.Add(card);
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

		public void ShowCommunityCards()
		{
			Console.WriteLine($"Общие карты: {string.Join(", ", CommunityCards)}");
		}
	}
}