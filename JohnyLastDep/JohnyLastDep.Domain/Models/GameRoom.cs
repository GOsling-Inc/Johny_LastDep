using JohnyLastDep.Domain.Entities;

namespace JohnyLastDep.Domain.Models
{
	public class GameRoom
	{
		public List<Player> Players { get; set; } = new List<Player>();
		public PokerGame Game { get; set; } = new PokerGame();
		public int IsReady { get; set; } = 0;
		public Player? CurrentPlayer { get; set; } = null;

		public GameRoom() 
		{
			Players = new List<Player>();
			Game = new PokerGame(Players);
			IsReady = 0;
			CurrentPlayer = null;
		}
	}
}
