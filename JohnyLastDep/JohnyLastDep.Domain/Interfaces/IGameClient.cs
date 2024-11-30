using JohnyLastDep.Domain.Entities;
using JohnyLastDep.Domain.Models;

namespace JohnyLastDep.Domain.Interfaces
{
	public interface IGameClient
	{
		Task ReceiveRooms(IEnumerable<string> rooms);
		Task ReceivePlayer(Player player);
		Task ReceiveGameState(PokerGame gameState);
		Task ReceiveBettingPlayer(Player bettingPlayer);
	}
}
