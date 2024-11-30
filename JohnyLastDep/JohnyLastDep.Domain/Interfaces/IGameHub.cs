namespace JohnyLastDep.Domain.Interfaces
{
	public interface IGameHub
	{
		Task CreateRoom(string roomName);
		Task GetRooms();
		Task JoinRoom(string roomName, string username);
		Task LeaveRoom(string roomName, string userId);
		Task StartGame(string roomName);
		Task GetBettingPlayer(string roomName);
		Task Bet(string roomName, string userId, int chips);
		Task Check(string roomName, string userId);
		Task Fold(string roomName, string userId);
		Task Reset(string roomName);
	}
}
