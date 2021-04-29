using System;
using System.Collections.Generic;
using System.Linq;

namespace TexasHoldem.Model
{
    public class Player
    {
        public Guid Id { get; private set; }
        public int Index { get; set; } = -1;
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Money { get; set; }
        public int BetMoney { get; set; }

        public GameRole Role { get; set; }
        public GamePlayerStatus Status { get; set; }

        public Poker Poker1 { get; set; } = null;
        public Poker Poker2 { get; set; } = null;

        public bool IsActive { get; set; } = false;

        public bool IsCheck { get; set; } = false;

        public bool IsWinner { get; set; } = false;

        public Round CurrentRound { get; set; } = Round.Default;
        public bool IsAllIn
        {
            get { return Money == 0; }
        }

        public PlayerStatus PlayerStatus { get; set; } = PlayerStatus.Audience;

        public Player(string name, string avatar)
        {
            Id = Guid.NewGuid();
            Name = name;
            Avatar = avatar;
            Money = 5000;
            Reset();
        }

        public void Reset()
        {
            this.Status = GamePlayerStatus.Play;
            this.Role = GameRole.Normal;
            this.IsCheck = false;
            this.IsActive = false;
            this.IsWinner = false;
            BetMoney = 0;
        }

        public override bool Equals(object other)
        {
            return this.Name == (other as Player).Name;
        }
    }
}