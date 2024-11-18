using System;
using System.Collections.Generic;
using System.Linq;
using Johny_LastDep.Domain.Entities;
using Johny_LastDep.Domain.Enums;
using Johny_LastDep.Domain.Models;

namespace Johny_LastDep
{
	class Program
	{
		static void Main(string[] args)
		{
			var playerNames = new List<string> { "Игрок 1", "Игрок 2", "Игрок 3" };
			var pokerGame = new PokerGame(playerNames);
			pokerGame.StartGame();
		}
	}

	public class PokerGame
	{
		public List<Player> Players { get; private set; }
		public Deck Deck { get; private set; }
		public Pot GamePot { get; private set; }
		public Table GameTable { get; private set; }
		private Dealer _dealer;
		private BettingManager _bettingManager;
		private GameState _gameState;

		public PokerGame(List<string> playerNames)
		{
			Players = playerNames.Select(name => new Player(name, 1000)).ToList(); 
			Deck = new Deck();
			GamePot = new Pot();
			GameTable = new Table(Players, 0, 10, 20, 10, 100); 
			_dealer = new Dealer(GameTable, GamePot); 
			_bettingManager = new BettingManager(Players, GamePot); 
			_gameState = new GameState(Players); 
		}

		public void StartGame()
		{
			_dealer.ShuffleDeck(); 
			_dealer.DealInitialCards(); 
			PlayRounds(); 
		}

		private void PlayRounds()
		{
			for (int i = 0; i < 3; i++) 
			{
				_gameState.IncrementRound(); 
				_bettingManager.ResetBets(); 
				_gameState.Pot = 0;

				DetermineCommunityCards(); 
				DetermineWinner();
				for(int j = 0; j < 3; j++)
				{
					Console.WriteLine($"{Players[j].Name}, {Players[j].Chips}");
				}
				ResetGame();
			}
		}

		private void DetermineCommunityCards()
		{
			_dealer.DealCommunityCards(3); 
			Console.WriteLine($"Флоп: {string.Join(", ", GameTable.CommunityCards)}");
			_bettingManager.PlaceBet(Players.First(), 10); 
			_gameState.UpdatePot(10);

			_dealer.DealCommunityCards(1); 
			Console.WriteLine($"Терн: {GameTable.CommunityCards.Last()}");
			_bettingManager.PlaceBet(Players.Last(), 20); 
			_gameState.UpdatePot(20);

			_dealer.DealCommunityCards(1); 
			Console.WriteLine($"Ривер: {GameTable.CommunityCards.Last()}");
			_bettingManager.PlaceBet(Players.First(), 30);
			_gameState.UpdatePot(30); 
		}

		private void DetermineWinner()
		{
			var winner = _dealer.DetermineWinner(Players);
			_dealer.DistributeWinnings(winner, _bettingManager._pot);
			if (winner != null)
			{
				_gameState.UpdatePot(GamePot.TotalAmount);
				var winnerMessage = winner.Count == 1
							? $"Победитель: {winner[0].Name} с рукой: {string.Join(", ", winner[0].HandCards.Select(card => card.ToString()))}!"
							: $"Ничья между игроками: {string.Join(", ", winner.Select(w => $"{w.Name} с рукой: {string.Join(", ", w.HandCards.Select(card => card.ToString()))}"))}";

				_gameState.AddAction(winnerMessage);
			}
			else
			{
				Console.WriteLine("Нет победителя!");
			}
		}

		private void ResetGame()
		{
			foreach (var player in Players)
			{
				player.ResetHand();
			}
			_dealer.DealInitialCards();
			GameTable.ResetCommunityCards(); 
			GamePot = new Pot(); 
			GameTable.MoveDealerPosition();
			_gameState.ResetGameState(Players); 
		}
	}
}