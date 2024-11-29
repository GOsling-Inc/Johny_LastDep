using Johny_LastDep.Domain.Entities;
using Johny_LastDep.Domain.Models;
public class PokerGame
	{
		public List<Player> Players { get; set; }
		public Table GameTable { get; set; }
		public Dealer Dealer { get; set; }
		public BettingManager BettingManager { get; set; }
		public GameState GameState { get; set; }
		public List<Player> Winners {get; set; }

		public PokerGame(List<Player> players)
		{
			Players = players;
			GameTable = new Table(Players, 0); 
			Dealer = new Dealer(GameTable); 
			BettingManager = new BettingManager(Players); 
			GameState = new GameState(Players); 
			Winners = [];
		}

		public void StartGame()
		{
			Dealer.ShuffleDeck(); 
		}

		public Player getBettingPlayer() {
			return GameState.ActivePlayers[GameState.CurrentPlayer];
		}

		public void Bet(string id, int amount) {
			BettingManager.PlaceBet(Players.Find((p) => p.Id == id)!, amount);
			GameState.Next();
			if (GameState.CurrentPlayer == GameState.InitialPosition) {
				if (BettingManager.CurrentBet == GameState.ActivePlayers[GameState.CurrentPlayer].CurrentBet) {
					Next();
				}
			}
		}

		public void Check(string id) {
			BettingManager.Check(Players.Find((p) => p.Id == id)!);
			GameState.Next();
			if (GameState.CurrentPlayer == GameState.InitialPosition) {
				if (BettingManager.CurrentBet == 0 && GameState.CurrentRound == 0) {
					throw new Exception("необходимо сделать ставку");
				}
				Next();
			}
		}

		public void Fold(string id) {
			GameState.RemovePlayer(Players.Find((p) => p.Id == id)!);
			GameState.Next();
			if (GameState.CurrentPlayer == GameState.InitialPosition) {
				if (BettingManager.CurrentBet == 0 && GameState.CurrentRound == 0) {
					throw new Exception("неоюходимо сделать ставку");
				}
				Next();
			}
		}

		private void Next() {
			GameState.IncrementRound();
			BettingManager.Next();
			Dealer.DealCards(GameState.CurrentRound);
			if (GameState.CurrentRound == 4 || GameState.ActivePlayers.Count() < 2) {
				Winners = Dealer.DetermineWinner(Players);
				Dealer.DistributeWinnings(Winners, BettingManager._pot);
			}
		}

		public void ResetGame()
		{
			foreach (var player in Players)
			{
				player.ResetHand();
			}
			GameTable.ResetCommunityCards(); 
			GameTable.MoveDealerPosition();
			BettingManager.ResetBets();
			GameState.ResetGameState(Players); 
			Winners.Clear();
		}
	}