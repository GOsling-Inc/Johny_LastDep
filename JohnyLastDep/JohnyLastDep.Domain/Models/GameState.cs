using JohnyLastDep.Domain.Entities;

namespace JohnyLastDep.Domain.Models
{
	public class GameState
	{
		public int CurrentRound { get; private set; }
		public int CurrentPlayer { get; private set; }
		public int InitialPosition { get; private set; }
		public List<Player> ActivePlayers { get; private set; }
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
		public void Next()
		{
			CurrentPlayer++;
			if (CurrentPlayer >= ActivePlayers.Count())
			{
				CurrentPlayer = 0;
			}
		}
		public void RemovePlayer(Player player)
		{
			ActivePlayers.Remove(player);
			if (InitialPosition >= ActivePlayers.Count())
			{
				InitialPosition--;
			}
		}
		public void ResetGameState(List<Player> players)
		{
			ActivePlayers = new List<Player>(players);
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