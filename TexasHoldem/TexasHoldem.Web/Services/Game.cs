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

    public enum Round
    {
        First = 1,
        Second,
        Third,
        Final
    }
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
        private Round _currentRound = Round.First;
        private IList<Poker> _openRiverPokers;
        private Poker _backPoker = new Poker(54);
        private int _smallIndex = -1;

        public Game(int id, IEnumerable<Player> players)
        {
            this.Id = id;
            foreach (var player in players)
            {
                player.SetBetChips();
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

            InitPoolChips();
        }

        private void InitPoolChips()
        {
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
            _openRiverPokers = new List<Poker>();
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
                RiverPokers.Add(_backPoker);
                _openRiverPokers.Add(AllPokers[index++]);
                Thread.Sleep(500);
                await GameUpdated?.Invoke(this);
            }
            Start();
        }

        protected void Start()
        {
            _smallIndex = this.Players.IndexOf(this.Players.FirstOrDefault(it => it.Role == GameRole.Small));
            _currentIndex = _smallIndex;
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

        public void SetPoolChips()
        {
            InitPoolChips();
            foreach (var player in Players)
            {
                foreach (var bet in player.BetChips)
                {
                    var poolChip = PoolChips.FirstOrDefault(it => it.Money == bet.Money);
                    if (poolChip != null)
                        poolChip.Num += bet.Num;
                }
            }

            if (CanGoNextRound())
            {
                GoToNextRound();
                _currentIndex = GetFirstIndex(_smallIndex);
                this.Players[_currentIndex].IsActive = true;
                PlayerUpdated?.Invoke(this.Players[_currentIndex]);
            }
            else
            {
                MoveToNext();
            }
        }

        private int GetFirstIndex(int index)
        {
            if (this.Players[index].Status != GamePlayerStatus.Fold)
                return index;
            index++;
            index = index % Players.Count();
            return GetFirstIndex(index);
        }

        private void GoToNextRound()
        {
            if (_currentRound == Round.First)
            {
                OpenThreeCards();
            }
            else if (_currentRound == Round.Second)
            {
                OpenFourthCards();
            }
            else if (_currentRound == Round.Third)
            {
                OpenFifthCards();
            }
            else
            {
                FinalCompute();
            }

            _currentRound = (Round)(_currentRound + 1);

        }

        private void FinalCompute()
        {

        }

        private bool CanGoNextRound()
        {
            var currentPlayers = Players.Where(it => it.Status == GamePlayerStatus.Play);
            foreach (var player in currentPlayers)
            {
                if (player.Money > 0)
                {
                    if (player.BetMoney < this.MaxBet)
                    {
                        return false;
                    }
                    continue;
                }
            }
            return true;
        }


        protected void OpenThreeCards()
        {
            RiverPokers[0] = _openRiverPokers[0];
            RiverPokers[1] = _openRiverPokers[1];
            RiverPokers[2] = _openRiverPokers[2];
        }

        protected void OpenFourthCards()
        {
            RiverPokers[3] = _openRiverPokers[3];
        }

        protected void OpenFifthCards()
        {
            RiverPokers[4] = _openRiverPokers[4];
        }

    }
}
