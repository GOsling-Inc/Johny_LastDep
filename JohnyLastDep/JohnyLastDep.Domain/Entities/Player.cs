namespace JohnyLastDep.Domain.Entities
{
	public class Player
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public List<Card> HandCards { get; set; }
		public int Chips { get; set; }
		public int CurrentBet { get; set; }
		public bool IsPlaying { get; set; }
		public bool IsReady { get; set; }

		public Player() { }

		public Player(string id, string name, int startingChips)
		{
			Id = id;
			Name = name;
			Chips = startingChips;
			HandCards = new List<Card>();
			CurrentBet = 0;
			IsPlaying = true;
			IsReady = false;
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
			IsReady = false;
			if(Chips < 100)
			{
				Chips = 500;
			}
		}
	}
}
