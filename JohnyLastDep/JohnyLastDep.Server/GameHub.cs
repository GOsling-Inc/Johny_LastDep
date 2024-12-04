using JohnyLastDep.Domain.Entities;
using JohnyLastDep.Domain.Models;
using JohnyLastDep.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace JohnyLastDep.Server
{
	public class GameHub : Hub<IGameClient>, IGameHub
	{
		private static Dictionary<string, GameRoom> rooms = [];

		public GameHub() {}
		public async Task CreateRoom(string roomName, string playerName)
		{
			rooms.Add(roomName, new GameRoom());
			await JoinRoom(roomName, playerName);
		}
		public async Task GetRooms()
		{
			await Clients.Caller.ReceiveRooms(rooms);
		}
		public async Task UpdateRooms()
		{
			await Clients.All.ReceiveRooms(rooms);
		}
		public async Task JoinRoom(string roomName, string userName)
		{
			if (!rooms.ContainsKey(roomName)) return;
			await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
			var player = new Player(Context.ConnectionId, userName, 1000);
			rooms[roomName].Players.Add(player);
			await Clients.Caller.ReceivePlayer(player);
			await UpdateRooms();
			await Clients.Group(roomName).ReceiveGameState(roomName, rooms[roomName].Game);
		}
		public async Task LeaveRoom(string roomName, string userId)
		{
			if (!rooms.ContainsKey(roomName)) return;
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
			var player = rooms[roomName].Players.Where((p) => p.Id == userId).First();
			rooms[roomName].Players.Remove(player);
			if (player.IsReady) rooms[roomName].IsReady -= 1;
			await UpdateRooms();
			await Clients.Group(roomName).ReceiveGameState(roomName, rooms[roomName].Game);
			await Clients.Caller.ReceivePlayer(null);

		}

		public async Task SetReady(string roomName, string userId, bool IsReady)
		{
			if (!rooms.ContainsKey(roomName)) return;
			var player = rooms[roomName].Players.Where((p) => p.Id == userId).First();
			player.IsReady = IsReady;
			await Clients.Caller.ReceivePlayer(player);
			rooms[roomName].IsReady += IsReady ? 1 : -1;
			await UpdateRooms();
			await Clients.Group(roomName).ReceiveGameState(roomName, rooms[roomName].Game);
		}
		public async Task StartGame(string roomName)
		{
			if (!rooms.ContainsKey(roomName)) return;
			rooms[roomName].Game = new PokerGame(rooms[roomName].Players);
			rooms[roomName].Game.StartGame();
			var currentPlayer = rooms[roomName].Game.GetBettingPlayer();
			rooms[roomName].CurrentPlayer = currentPlayer;
			await UpdateRooms();
			await Clients.Group(roomName).ReceiveBettingPlayer(roomName, currentPlayer);
			await Clients.Group(roomName).ReceiveGameState(roomName, rooms[roomName].Game);
		}
		public async Task GetBettingPlayer(string roomName)
		{
			await Clients.Group(roomName).ReceiveBettingPlayer(roomName, rooms[roomName].Game.GetBettingPlayer());
		}

		public async Task GetGameState(string roomName)
		{
			await Clients.Group(roomName).ReceiveGameState(roomName, rooms[roomName].Game);
		}

		public async Task UpdateRoomPlayers(string roomName)
		{
			foreach(var player in rooms[roomName].Players)
			{
				await Clients.Client(player.Id).ReceivePlayer(player);
			}
		}

		public async Task Bet(string roomName, string userId, int chips)
		{
			if (!rooms.ContainsKey(roomName)) return;
			rooms[roomName].Game.Bet(userId, chips);
			var game = rooms[roomName].Game;
			await Clients.Group(roomName).ReceiveRooms(rooms);
			await Clients.Group(roomName).ReceiveGameState(roomName, game);
			await Clients.Group(roomName).ReceiveBettingPlayer(roomName,game.GetBettingPlayer());
			await UpdateRoomPlayers(roomName);
		}
		public async Task Check(string roomName, string userid)
		{
			if (!rooms.ContainsKey(roomName)) return;
			rooms[roomName].Game.Check(userid);
			await Clients.Group(roomName).ReceiveRooms(rooms);
			await Clients.Group(roomName).ReceiveGameState(roomName, rooms[roomName].Game);
			await Clients.Group(roomName).ReceiveBettingPlayer(roomName, rooms[roomName].Game.GetBettingPlayer());
			await UpdateRoomPlayers(roomName);
		}
		public async Task Fold(string roomName, string userid)
		{
			if (!rooms.ContainsKey(roomName)) return;
			rooms[roomName].Game.Fold(userid);
			await Clients.Group(roomName).ReceiveGameState(roomName, rooms[roomName].Game);
			await Clients.Group(roomName).ReceiveBettingPlayer(roomName, rooms[roomName].Game.GetBettingPlayer());
			await Clients.Group(roomName).ReceiveRooms(rooms);
			await UpdateRoomPlayers(roomName);
		}
		public async Task Reset(string roomName)
		{
			if (rooms.ContainsKey(roomName))
			{
				rooms[roomName].IsReady = 0;
				rooms[roomName].CurrentPlayer = null;
				rooms[roomName].Game.ResetGame();
				await Clients.Group(roomName).ReceiveGameState(roomName, rooms[roomName].Game);
				await Clients.Group(roomName).ReceiveBettingPlayer(roomName, null);
				await UpdateRoomPlayers(roomName);
				await UpdateRooms();
			}
		}

	}
}
