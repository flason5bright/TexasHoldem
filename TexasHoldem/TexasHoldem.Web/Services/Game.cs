using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TexasHoldem.Model;

namespace TexasHoldem.Web.Services
{
    public delegate Task GameUpatedHandler(Game game);
    public delegate Task PlayerUpatedHandler(Player player);
    public delegate Task RiverPokerDealingHandler(Poker poker);
    public delegate Task DealPokerHandler(Player player);
    public class Game
    {
        public int Id { get; private set; }
        public IList<Poker> RiverPokers { get; set; }
        public IList<Player> Players { get; private set; } = new List<Player>();

        public IList<Poker> AllPokers { get; private set; }

        public GameStatus GameStatus { get; set; }

        public IList<Chip> PoolChips { get; set; }

        public int MaxBet { get; set; }

        public int PoolMoney { get { return PoolChips.Sum(it => it.Money * it.Num); } }

        private int _currentIndex = -1;

        public Game(int id, IEnumerable<Player> players)
        {
            this.Id = id;
            foreach (var player in players)
            {
                Players.Add(player);
            }
            foreach (var player in Players)
            {
                player.PlayerStatus = PlayerStatus.Gaming;
            }

            var count = Players.Count();
            var buttonIndex = this.Id % count;
            var smallIndex = (this.Id + 1) % count;
            var bigIndex = (this.Id + 2) % count;

            Players[buttonIndex].Role = GameRole.Button;
            Players[smallIndex].Role = GameRole.Small;
            Players[bigIndex].Role = GameRole.Big;

            PoolChips = new List<Chip>()
            {
                new Chip(5,0),
                new Chip(25,0),
                new Chip(50,0),
                new Chip(100,0),
                new Chip(500,0),
            };
        }

        public async Task StartGame()
        {
            GameStatus = GameStatus.Started;

            await GameStarted?.Invoke(this);

        }
        public event GameUpatedHandler GameUpdated;
        public event PlayerUpatedHandler PlayerUpdated;

        public event GameUpatedHandler GameStarted;
        public event GameUpatedHandler Shuffling;
        public event GameUpatedHandler Shuffled;
        public event DealPokerHandler Dealing;
        public event RiverPokerDealingHandler RiverPokerDealing;

        private void InitailizePokers()
        {
            AllPokers = new List<Poker>();
            int index = 0;
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (PokerSize size in Enum.GetValues(typeof(PokerSize)))
                {
                    AllPokers.Add(new Poker(index++, size, suit));
                }
            }
        }



        public async Task Shuffle()
        {
            this.GameStatus = GameStatus.Shuffling;
            await Shuffling?.Invoke(this);
            Thread.Sleep(500);
            InitailizePokers();
            await Shuffled?.Invoke(this);
        }

        public async Task DealCards()
        {
            RiverPokers = new List<Poker>();
            int index = 0;
            foreach (var player in Players)
            {
                player.Poker1 = AllPokers[index++];
                player.Poker2 = AllPokers[index++];
                await Dealing?.Invoke(player);
                Thread.Sleep(500);
            }

            for (int i = 0; i < 5; i++)
            {
                RiverPokers.Add(AllPokers[index++]);
                Thread.Sleep(500);
                await GameUpdated?.Invoke(this);
            }
            StartFirstRound();
        }

        protected void StartFirstRound()
        {
            var firstIndex = this.Players.IndexOf(this.Players.FirstOrDefault(it => it.Role == GameRole.Small));
            _currentIndex = firstIndex;
            this.Players[_currentIndex].IsActive = true;
            PlayerUpdated?.Invoke(this.Players[_currentIndex]);

        }

        public void MoveToNext()
        {
            _currentIndex++;
            _currentIndex = _currentIndex % Players.Count();
            this.Players[_currentIndex].IsActive = true;
            PlayerUpdated?.Invoke(this.Players[_currentIndex]);
        }


        protected void OpenThreeCards()
        {
            //开前三张牌
        }

    }
}
