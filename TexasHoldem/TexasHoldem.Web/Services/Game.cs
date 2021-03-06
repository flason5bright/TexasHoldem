﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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

        public IList<Poker> AllPokers { get { return PokerService.Instance.AllPokers; } }

        public List<FinalPoker> FinalPokers { get; set; } = new List<FinalPoker>();

        public GameStatus GameStatus { get; set; }

        public int MaxBet { get; set; }

        public int PoolMoney { get; set; }
        public Round CurrentRound { get; set; } = Round.First;

        public bool IsFinished { get; set; } = false;
        private int _currentIndex = -1;

        private IList<Poker> _openRiverPokers;
        private Poker _backPoker = new Poker(54);
        private int _smallIndex = -1;

        public Game(int id, IEnumerable<Player> players)
        {
            this.Id = id;
            foreach (var player in players)
            {
                player.Reset();
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
            PoolMoney = 0;
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
            Thread.Sleep(500);
            PokerService.Instance.Shuffle();
            await Shuffled?.Invoke(this);
        }

        public async Task DealCards()
        {
            GameStatus = GameStatus.Playing;
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


        /// <summary>
        /// 移动到下个玩家
        /// </summary>
        public void MoveToNext()
        {
            _currentIndex++;
            _currentIndex = _currentIndex % Players.Count();
            if (this.Players[_currentIndex].Status == GamePlayerStatus.Fold || this.Players[_currentIndex].IsAllIn)
                MoveToNext();
            else
            {
                this.Players[_currentIndex].IsActive = true;
                PlayerUpdated?.Invoke(this.Players[_currentIndex]);
            }

        }

        /// <summary>
        /// 判断是不是都check了
        /// </summary>
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

        /// <summary>
        /// 一个玩家下注完计算底池
        /// </summary>
        public void SetPoolChips()
        {
            InitPoolChips();
            PoolMoney = Players.Sum(it => it.BetMoney);
            Next();

        }

        /// <summary>
        /// 下一步
        /// </summary>
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

        /// <summary>
        /// 查找每轮第一个下注的玩家
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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
            if (CurrentRound == Round.First)
            {
                OpenThreeCards();
            }
            else if (CurrentRound == Round.Second)
            {
                OpenFourthCards();
            }
            else if (CurrentRound == Round.Third)
            {
                OpenFifthCards();
            }
            else
            {
                List<FinalPoker> pokers = new List<FinalPoker>();
                var currentPlayers = Players.Where(it => it.Status == GamePlayerStatus.Play);
                foreach (var player in currentPlayers)
                {
                    player.IsCheck = false;
                    pokers.Add(new FinalPoker(player.Poker1, player.Poker2, RiverPokers, player.Name));
                }

                FinalCompute(pokers);
                GameStatus = GameStatus.Finished;
                return;
            }

            CurrentRound = (Round)(CurrentRound + 1);

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
            this.FinalPokers.AddRange(PokerService.Instance.Summary(pokers));
            var result = this.FinalPokers.GroupBy(it => it.Result);

            var totalMoney = this.PoolMoney;
            foreach (var item in result)
            {
                if (totalMoney == 0)
                    break;
                var count = item.Count();
                foreach (var fp in item)
                {
                    var winMoney = GetChipMoney(totalMoney / count);
                    var player = Players.FirstOrDefault(it => it.Name == fp.PlayerName);
                    player.IsWinner = true;
                    if (player.IsAllIn)
                    {
                        if (player.BetMoney <= winMoney)
                        {
                            totalMoney -= player.Money;
                            AllInPlayerWinMoney(player);
                        }
                        else
                        {
                            PlayerWinMoney(player, winMoney);
                            totalMoney -= winMoney;
                        }
                    }
                    else
                    {
                        PlayerWinMoney(player, winMoney);
                        totalMoney -= winMoney;
                    }
                }
            }

            PlayersUpdated?.Invoke(this.Players[_currentIndex]);
        }

        private void AllInPlayerWinMoney(Player player)
        {
            player.Money = player.Money * 2;
        }

        private void PlayerWinMoney(Player player, int money)
        {
            player.Money += money;
        }

        /// <summary>
        /// 33.3333,55.55
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int GetChipMoney(float input)
        {
            var input1 = (int)Math.Ceiling(input);
            var last = input1 % 10;
            if (last == 5 || last == 0)
                return input1;
            if (last < 5)
                return input1 - last + 5;
            if (last > 5)
                return input1 - last + 10;
            return input1;
        }

        private void GoToFinal()
        {
            CurrentRound = Round.Final;
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
                if (player.IsAllIn)
                    player.CurrentRound = this.CurrentRound;

                if (player.CurrentRound < this.CurrentRound)
                    return false;

                if (!player.IsAllIn)
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
}
