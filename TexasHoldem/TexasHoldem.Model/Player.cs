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
        public int Money { get { return Chips.Sum(it => it.Money * it.Num); } }
        public int BetMoney { get { return BetChips.Sum(it => it.Money * it.Num); } }

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

        public IEnumerable<Chip> Chips { get; set; }

        public IEnumerable<Chip> BetChips { get; set; }

        public PlayerStatus PlayerStatus { get; set; } = PlayerStatus.Audience;

        public Player(string name, string avatar)
        {
            Id = Guid.NewGuid();
            Name = name;
            Avatar = avatar;
            Chips = new List<Chip>()
            {
                new Chip(5,4),
                new Chip(25,4),
                new Chip(50,4),
                new Chip(100,4),
                new Chip(500,4),
            };
            Reset();
        }

        public void Reset()
        {
            this.IsCheck = false;
            this.IsActive = false;
            this.IsWinner = false;
            BetChips = new List<Chip>()
            {
                new Chip(5,0),
                new Chip(25,0),
                new Chip(50,0),
                new Chip(100,0),
                new Chip(500,0),
            };
        }

        public override bool Equals(object other)
        {
            return this.Name == (other as Player).Name;
        }
    }
}