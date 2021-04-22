using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TexasHoldem.Model;

namespace TexasHoldem.Web.Services
{
    public class GameEngine
    {



    }

    public enum GameStatus
    {
        NotStarted = 0,
        Started,
        Shuffling,
        Shuffed,
        Dealing,
        Dealed,
        playing,
    }

    public class GamePlayer
    {
        public Player Player { get; private set; }
        public GameRole Role { get; set; }
        public GamePlayerStatus Status { get; set; }

        public Poker Poker1 { get; set; } = null;
        public Poker Poker2 { get; set; } = null;

        public GamePlayer(Player player)
        {
            Player = player;
        }
    }



}
