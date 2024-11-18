using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Johny_LastDep.Domain.Enums;

namespace Johny_LastDep.Domain.Entities
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
