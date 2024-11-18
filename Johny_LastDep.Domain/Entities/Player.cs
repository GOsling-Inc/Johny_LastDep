using Johny_LastDep.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Johny_LastDep.Domain.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> HandCards { get; set; }
        public int Chips { get; set; }
        public int CurrentBet { get; set; }
        public bool IsPlaying { get; set; }
        public List<PlayerAction> ActionHistory { get; set; }

        public Player(string name, int startingChips)
        {
            Name = name;
            Chips = startingChips;
            HandCards = new List<Card>();
            CurrentBet = 0;
            IsPlaying = true;
            ActionHistory = new List<PlayerAction>();
        }

        public void Fold()
        {
            IsPlaying = false;
            ActionHistory.Add(new PlayerAction(PlayerActionType.Fold, 0));
        }

        public void Call(int callAmount)
        {
            int betAmount = callAmount - CurrentBet;
            Chips -= betAmount;
            CurrentBet = callAmount;
            ActionHistory.Add(new PlayerAction(PlayerActionType.Call, betAmount));
        }

        public void Raise(int raiseAmount)
        {
            int betAmount = raiseAmount - CurrentBet;
            Chips -= betAmount;
            CurrentBet = raiseAmount;
            ActionHistory.Add(new PlayerAction(PlayerActionType.Raise, betAmount));
        }

        public void ResetHand()
        {
            HandCards.Clear();
            CurrentBet = 0;
            IsPlaying = true;
        }
    }
}
