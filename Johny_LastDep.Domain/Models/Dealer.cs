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
		public Pot _pot { get; private set; }

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

		public List<Player> DetermineWinner(List<Player> players)
		{
			var evaluator = new HandEvaluator();
			List<Player> winners = new List<Player>();
			Hand winningHand = null;

			foreach (var player in players.Where(p => p.IsPlaying))
			{
				var playerHand = evaluator.EvaluateHand(player.HandCards, _table.CommunityCards);
				Console.WriteLine($"{player.Name} имеет комбинацию: {playerHand.Type}");

				if (winningHand == null || Hand.CompareHands(playerHand, winningHand) > 0)
				{
					winningHand = playerHand;
					winners.Clear(); 
					winners.Add(player);
				}
				else if (winningHand != null && Hand.CompareHands(playerHand, winningHand) == 0)
				{
					winners.Add(player);
				}
			}

			return winners;
		}

		public void DistributeWinnings(List<Player> winners, Pot pot)
		{
			_pot = pot;
			if (winners.Count > 0)
			{
				if (winners.Count == 1)
				{
					Player winner = winners[0];
					Console.WriteLine($"Победитель: {winner.Name} с рукой: {string.Join(", ", winner.HandCards.Select(card => card.ToString()))}!");
					Console.WriteLine($"Сумма в поте перед распределением: {_pot.TotalAmount}");
					winner.Chips += _pot.TotalAmount; 
					Console.WriteLine($"Итоговый банк {winner.Name}: {winner.Chips}");
				}
				else
				{
					Console.WriteLine("Ничья между игроками:");
					foreach (var winner in winners)
					{
						Console.WriteLine($"{winner.Name} с рукой: {string.Join(", ", winner.HandCards.Select(card => card.ToString()))}!");
					}

					int amountPerPlayer = _pot.TotalAmount / winners.Count;
					foreach (var winner in winners)
					{
						winner.Chips += amountPerPlayer;
						Console.WriteLine($"Каждому победителю возвращается {amountPerPlayer}. Итоговый банк {winner.Name}: {winner.Chips}");
					}
				}
			}
			else
			{
				Console.WriteLine("Нет победителей!");
			}
		}
	}
}