using JohnyLastDep.Domain.Entities;

namespace JohnyLastDep.Domain.Models
{
	public class GameState
	{
		public int CurrentRound { get; set; } = 0;
		public int CurrentPlayer { get; set; } = 0;
		public int InitialPosition { get; set; } = 0;
		public List<Player> ActivePlayers { get; set; } = new List<Player>();
		public GameState() { }
		public GameState(List<Player> players)
		{
			CurrentRound = 0;
			InitialPosition = 0;
			CurrentPlayer = 0;
			ActivePlayers = new List<Player>(players);
		}
		public void IncrementRound()
		{
			CurrentRound++;
			CurrentPlayer = InitialPosition;
		}
		public void Next(bool isFold = false)
		{
			CurrentPlayer++;
			var players = ActivePlayers.Count();
			if (CurrentPlayer >= players)
			{
				if(isFold) CurrentPlayer = players - 1;
				else CurrentPlayer = 0;
			}
		}
		public void RemovePlayer(Player player)
		{
			ActivePlayers.Remove(player);
			player.IsPlaying = false;
			if (InitialPosition >= ActivePlayers.Count())
			{
				--InitialPosition;
			}
		}
		public void ResetGameState(List<Player> players)
		{
			ActivePlayers = new List<Player>();
			CurrentRound = 0;
			InitialPosition++;
			if (InitialPosition >= ActivePlayers.Count())
			{
				InitialPosition = 0;
			}
			CurrentPlayer = InitialPosition;
		}
	}
}