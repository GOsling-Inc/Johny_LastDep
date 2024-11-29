using Johny_LastDep.Domain.Models;
using Johny_LastDep.Domain.Entities;

namespace Johny_LastDep.Domain.Interfaces;

public interface IGameClient
{
    Task ReceiveRooms(IEnumerable<string> rooms);
    Task ReceivePlayer(Player player);
    Task ReceiveGameState(PokerGame gameState);
    Task ReceiveBettingPlayer(Player bettingPlayer);
}