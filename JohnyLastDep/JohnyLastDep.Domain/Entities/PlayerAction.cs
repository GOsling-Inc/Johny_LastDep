using JohnyLastDep.Domain.Enums;

namespace JohnyLastDep.Domain.Entities
{
	public class PlayerAction
	{
		public PlayerActionType ActionType { get; set; }
		public int BetAmount { get; set; }

		public PlayerAction(PlayerActionType actionType, int betAmount)
		{
			ActionType = actionType;
			BetAmount = betAmount;
		}
	}
}
