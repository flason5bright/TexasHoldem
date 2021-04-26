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

        public IList<Poker> AllPokers { get { return PokerService.Instance.AllPokers; } }

        public GameStatus GameStatus { get; set; }

        public IList<Chip> PoolChips { get; set; }

        public int MaxBet { get; set; }

        public int PoolMoney { get { return PoolChips.Sum(it => it.Money * it.Num); } }

        public bool IsFinished { get; set; } = false;

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
        public event PlayerUpatedHandler PlayersUpdated;

        public event GameUpatedHandler GameStarted;
        public event GameUpatedHandler Shuffling;
        public event GameUpatedHandler Shuffled;
        public event DealPokerHandler Dealing;





        public async Task Shuffle()
        {
            this.GameStatus = GameStatus.Shuffling;
            await Shuffling?.Invoke(this);
            Thread.Sleep(500);
            PokerService.Instance.Shuffle();
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
            if (this.Players[_currentIndex].Status == GamePlayerStatus.Fold)
                MoveToNext();
            else
            {
                this.Players[_currentIndex].IsActive = true;
                PlayerUpdated?.Invoke(this.Players[_currentIndex]);
            }

        }

        public void IsAllCheck()
        {
            var currentPlayers = Players.Where(it => it.Status == GamePlayerStatus.Play);
            if (!currentPlayers.Any(it => it.IsCheck == false))
            {
                GoToNextRound();
            }
            else
            {
                MoveToNext();
            }
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
            Next();

        }
        public void Next()
        {
            if (CanGoNextRound())
            {
                GoToNextRound();
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
                List<FinalPoker> pokers = new List<FinalPoker>();
                var currentPlayers = Players.Where(it => it.Status == GamePlayerStatus.Play);
                foreach (var player in currentPlayers)
                {
                    pokers.Add(new FinalPoker(player.Poker1, player.Poker2, RiverPokers, player.Id));
                }

                FinalCompute(pokers);
                return;
            }

            _currentRound = (Round)(_currentRound + 1);

            foreach (var item in Players)
            {
                item.IsCheck = false;
            }

            _currentIndex = GetFirstIndex(_smallIndex);
            this.Players[_currentIndex].IsActive = true;
            PlayersUpdated?.Invoke(this.Players[_currentIndex]);

        }

        private void FinalCompute(List<FinalPoker> pokers)
        {
            this.IsFinished = true;
            var result = PokerService.Instance.Summary(pokers).GroupBy(it => it.Result);
            foreach (var item in result)
            {
                foreach (var fp in item)
                {
                    var player = Players.FirstOrDefault(it => it.Id == fp.PlayerId);
                    player.IsWinner = true;
                    foreach (var chip in this.PoolChips)
                    {
                        var pChip = player.Chips.FirstOrDefault(it => it.Money == chip.Money);
                        pChip.Num += chip.Num;
                    }
                    break;
                }

                break;
            }

            PlayersUpdated?.Invoke(this.Players[_currentIndex]);
        }

        private void GoToFinal()
        {
            _currentRound = Round.Final;
            OpenAll();
        }

        private bool CanGoNextRound()
        {
            var currentPlayers = Players.Where(it => it.Status == GamePlayerStatus.Play);
            if (currentPlayers.Count() == 1)
            {
                GoToFinal();
                return true;
            }
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

        protected void OpenAll()
        {
            RiverPokers[0] = _openRiverPokers[0];
            RiverPokers[1] = _openRiverPokers[1];
            RiverPokers[2] = _openRiverPokers[2];
            RiverPokers[3] = _openRiverPokers[3];
            RiverPokers[4] = _openRiverPokers[4];
        }

    }

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
