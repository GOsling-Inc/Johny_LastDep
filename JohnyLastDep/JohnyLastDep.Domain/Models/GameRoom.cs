using JohnyLastDep.Domain.Entities;

namespace JohnyLastDep.Domain.Models
{
	public class GameRoom
	{
		public List<Player> Players { get; set; } = new List<Player>();
		public PokerGame Game { get; set; } = new PokerGame(new List<Player>());
		public int IsReady { get; set; } = 0;
		public Player? CurrentPlayer { get; set; } = null;

		public GameRoom() { }
	}
}
