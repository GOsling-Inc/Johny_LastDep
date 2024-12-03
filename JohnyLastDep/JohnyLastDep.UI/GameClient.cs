using JohnyLastDep.Domain.Entities;
using JohnyLastDep.Domain.Interfaces;
using JohnyLastDep.Domain.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace JohnyLastDep.UI
{
	public class GameClient : IGameClient
	{
		private readonly HubConnection _connection;
		public Dictionary<string, GameRoom> Rooms { get; set; } = [];
		public Player? Player { get; set; } = null;

		public string PlayerAction { get; set; } = "";

		public event Action OnRoomsUpdated;
		public event Action OnPlayerUpdated;
		public event Action<string> OnGameStateUpdated;

		public GameClient(HubConnection connection) 
        { 
            _connection = connection;
            _connection.On<Dictionary<string, GameRoom>>("ReceiveRooms", ReceiveRooms);
			_connection.On<Player?>("ReceivePlayer", ReceivePlayer);
			_connection.On<string, PokerGame>("ReceiveGameState", ReceiveGameState);
			_connection.On<string, Player>("ReceiveBettingPlayer", ReceiveBettingPlayer);
        }

		public async Task GetRooms()
		{
			await _connection.InvokeAsync("GetRooms");
		}

		public async Task ReceiveRooms(Dictionary<string, GameRoom> rooms)
		{
			Rooms = rooms;
			OnRoomsUpdated?.Invoke();
		}
		public async Task CreateRoom(string roomName, string playerName) 
		{
			await _connection.InvokeAsync("CreateRoom", roomName, playerName);
		}

		public async Task JoinRoom(string roomName, string playerName)
		{
			await _connection.InvokeAsync("JoinRoom", roomName, playerName);
		}

		public async Task LeaveRoom(string roomName, string playerId)
		{
			await _connection.InvokeAsync("LeaveRoom", roomName, playerId);
		}

		public async Task SetReady(string roomName, string playerId,  bool IsReady)
		{
			await _connection.InvokeAsync("SetReady", roomName, playerId, IsReady);
			OnPlayerUpdated?.Invoke();
			OnGameStateUpdated?.Invoke(roomName);
		}

		public async Task ReceivePlayer(Player? player)
		{
			Player = player;
			OnPlayerUpdated?.Invoke();
		}

		public async Task ReceiveGameState(string roomName, PokerGame gameState)
		{
			Rooms[roomName].Game = gameState;
			OnGameStateUpdated?.Invoke(roomName);
		}

		public async Task ReceiveBettingPlayer(string roomName, Player? bettingPlayer)
		{
			Rooms[roomName].CurrentPlayer = bettingPlayer;
			OnGameStateUpdated?.Invoke(roomName);
		}

		public async Task StartGame(string roomName)
		{
			await _connection.InvokeAsync("StartGame", roomName);
			OnPlayerUpdated?.Invoke();
			OnGameStateUpdated?.Invoke(roomName);
		}

		public async Task Bet(string roomName, string userId, int chips)
		{
			await _connection.InvokeAsync("Bet", roomName, userId, chips);
		}

		public async Task Check(string roomName, string userId)
		{
			await _connection.InvokeAsync("Check", roomName, userId);
		}

		public async Task Fold(string roomName, string userId)
		{
			await _connection.InvokeAsync("Fold", roomName, userId);
		}

		public async Task ResetGame(string roomName)
		{
			await _connection.InvokeAsync("Reset", roomName);
		}
	}
}
