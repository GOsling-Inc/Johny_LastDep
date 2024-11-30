using JohnyLastDep.Domain.Entities;
using JohnyLastDep.Domain.Models;
using JohnyLastDep.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace JohnyLastDep.Server
{
	public class Room
	{
		public List<Player> Players = [];
		public PokerGame game = new([]);
	}
	public class GameHub : Hub<IGameClient>, IGameHub
	{
		private static Dictionary<string, Room> rooms = [];

		public GameHub() {}
		public async Task CreateRoom(string roomName)
		{
			rooms.Add(roomName, new Room());
			await Clients.All.ReceiveRooms(rooms.Keys);
		}
		public async Task GetRooms()
		{
			await Clients.Caller.ReceiveRooms(rooms.Keys);
		}
		public async Task JoinRoom(string roomName, string username)
		{
			if (rooms.ContainsKey(roomName))
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

				var player = new Player(username, 1000);
				rooms[roomName].Players.Add(player);
				rooms[roomName].game.Players.Add(player);
				await Clients.Caller.ReceivePlayer(player);
				await Clients.Group(roomName).ReceiveGameState(rooms[roomName].game);
			}
		}
		public async Task LeaveRoom(string roomName, string userid)
		{
			if (rooms.ContainsKey(roomName))
			{
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

				rooms[roomName].Players.Remove(rooms[roomName].Players.Where((p) => p.Id == userid).First());

				await Clients.Group(roomName).ReceiveGameState(rooms[roomName].game);
			}
		}
		public async Task StartGame(string roomName)
		{
			if (rooms.ContainsKey(roomName))
			{
				rooms[roomName].game = new PokerGame(rooms[roomName].Players);

				rooms[roomName].game.StartGame();

				await Clients.Group(roomName).ReceiveGameState(rooms[roomName].game);

				await Clients.Group(roomName).ReceiveBettingPlayer(rooms[roomName].game.getBettingPlayer());
			}
		}
		public async Task GetBettingPlayer(string roomName)
		{
			await Clients.Group(roomName).ReceiveBettingPlayer(rooms[roomName].game.getBettingPlayer());
		}
		public async Task Bet(string roomName, string userid, int chips)
		{
			if (rooms.ContainsKey(roomName))
			{
				rooms[roomName].game.Bet(userid, chips);

				await Clients.Group(roomName).ReceiveGameState(rooms[roomName].game);
				await Clients.Group(roomName).ReceiveBettingPlayer(rooms[roomName].game.getBettingPlayer());
			}
		}
		public async Task Check(string roomName, string userid)
		{
			if (rooms.ContainsKey(roomName))
			{
				rooms[roomName].game.Check(userid);

				await Clients.Group(roomName).ReceiveGameState(rooms[roomName].game);
				await Clients.Group(roomName).ReceiveBettingPlayer(rooms[roomName].game.getBettingPlayer());
			}
		}
		public async Task Fold(string roomName, string userid)
		{
			if (rooms.ContainsKey(roomName))
			{
				rooms[roomName].game.Fold(userid);

				await Clients.Group(roomName).ReceiveGameState(rooms[roomName].game);
				await Clients.Group(roomName).ReceiveBettingPlayer(rooms[roomName].game.getBettingPlayer());
			}
		}
		public async Task Reset(string roomName)
		{
			if (rooms.ContainsKey(roomName))
			{
				rooms[roomName].game.ResetGame();

				rooms[roomName].game.StartGame();

				await Clients.Group(roomName).ReceiveGameState(rooms[roomName].game);

				await Clients.Group(roomName).ReceiveBettingPlayer(rooms[roomName].game.getBettingPlayer());
			}
		}

	}
}
