using System;
using System.Collections.Generic;
using TexasHoldem.Model;

namespace TexasHoldem.Web.Services
{
    public class FinalPoker
    {
        public List<Poker> Pokers { get; }

        public float Result { get; set; }

        public Guid PlayerId { get; set; }
        public FinalPoker(Poker poker1, Poker poker2, IList<Poker> riverPoker, Guid playerId)
        {
            this.PlayerId = playerId;
            Pokers = new List<Poker>();
            Pokers.Add(poker1);
            Pokers.Add(poker2);
            Pokers.AddRange(riverPoker);
        }
    }
}
