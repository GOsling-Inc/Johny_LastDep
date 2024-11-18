using System;
using System.Collections.Generic;
using System.Linq;
using Johny_LastDep.Domain.Entities;

namespace Johny_LastDep.Domain.Models
{
	public class Dealer
	{
		private Deck _deck;
		private Table _table;
		private Pot _pot;

		public Dealer(Table table, Pot pot)
		{
			_table = table;
			_pot = pot;
			_deck = new Deck();
		}

		public void ShuffleDeck()
		{
			_deck.Shuffle();
		}

		public void DealInitialCards()
		{
			foreach (var player in _table.Players)
			{
				player.HandCards.Add(_deck.Deal());
				player.HandCards.Add(_deck.Deal());
				Console.WriteLine($"{player.Name} получает: {player.HandCards[0]} и {player.HandCards[1]}");
			}
		}

		public void DealCommunityCards(int numberOfCards)
		{
			for (int i = 0; i < numberOfCards; i++)
			{
				var card = _deck.Deal();
				_table.AddCommunityCard(card);
			}
		}

		public Player DetermineWinner(List<Player> players)
		{
			var evaluator = new HandEvaluator();
			Player winner = null;
			Hand winningHand = null;

			foreach (var player in players.Where(p => p.IsPlaying))
			{
				var playerHand = evaluator.EvaluateHand(player.HandCards, _table.CommunityCards);
				Console.WriteLine($"{player.Name} имеет комбинацию: {playerHand.Type}");

				if (winningHand == null || Hand.CompareHands(playerHand, winningHand) > 0)
				{
					winningHand = playerHand;
					winner = player;
				}
			}

			return winner;
		}

		public void DistributeWinnings(Player winner)
		{
			if (winner != null)
			{
				Console.WriteLine($"Победитель: {winner.Name} с рукой: {string.Join(", ", winner.HandCards.Select(card => card.ToString()))}!");
				winner.Chips += _pot.TotalAmount;
			}
			else
			{
				Console.WriteLine("Нет победителя!");
			}
		}
	}
}