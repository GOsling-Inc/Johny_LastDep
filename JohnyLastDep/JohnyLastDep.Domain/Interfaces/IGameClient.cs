using JohnyLastDep.Domain.Entities;
using JohnyLastDep.Domain.Models;

namespace JohnyLastDep.Domain.Interfaces
{
	public interface IGameClient
	{
		Task ReceiveRooms(Dictionary<string, GameRoom> rooms);
		Task ReceivePlayer(Player? player);
		Task ReceiveGameState(string roomName, PokerGame gameState);
		Task ReceiveBettingPlayer(string roomName, Player? bettingPlayer);
	}
}
