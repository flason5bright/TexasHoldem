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
        public decimal Money { get { return Chips.Sum(it => it.Money * it.Num); } }

        public GameRole Role { get; set; }
        public GamePlayerStatus Status { get; set; }

        public Poker Poker1 { get; set; } = null;
        public Poker Poker2 { get; set; } = null;

        public bool IsActive { get; set; } = false;



        public IEnumerable<Chip> Chips { get; set; }

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
        }

        public override bool Equals(object other)
        {
            return this.Name == (other as Player).Name;
        }
    }
    public class Chip
    {
        public int Money { get; set; }

        public int Num { get; set; }
        public Chip(int money, int num)
        {
            this.Money = money;
            this.Num = num;

        }

    }
}