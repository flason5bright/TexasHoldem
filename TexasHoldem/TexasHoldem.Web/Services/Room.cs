using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TexasHoldem.Model;
using TexasHoldem.Web.Hubs;
using TexasHoldem.Web.Services;

namespace TexasHoldem.Web
{
    public class Room
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }

        public IList<Player> Players { get; private set; }

        public Game CurrentGame { get; set; }

        public int GameIndex { get; private set; } = 0;

        private readonly IHubContext<GameHub, IGameHub> _hubContext;

        public async Task StartNewGame()
        {
            CurrentGame = new Game(GameIndex++, Players.Where(it => it.PlayerStatus >= PlayerStatus.OnSeat).OrderBy(it=>it.Index));
            CurrentGame.GameStarted += CurrentGame_GameInitialized;
            CurrentGame.PlayerUpdated += CurrentGame_PlayerUpdated;
            CurrentGame.Shuffled += CurrentGame_Shuffled;
            CurrentGame.Shuffling += CurrentGame_Shuffled;
            CurrentGame.Dealing += CurrentGame_Dealing;
            CurrentGame.GameUpdated += CurrentGame_GameUpdated;
            CurrentGame.PlayersUpdated += CurrentGame_PlayersUpdated;



            await CurrentGame.StartGame()
                .ContinueWith((obj) => CurrentGame.Shuffle())
                 .ContinueWith((obj) => CurrentGame.DealCards());
        }

        private async Task CurrentGame_PlayersUpdated(Player player)
        {
            await _hubContext.Clients.All.UpdatePlayers(CurrentGame.Players);
        }

        private async Task CurrentGame_PlayerUpdated(Player player)
        {
            await _hubContext.Clients.All.UpdatePlayer(player);
        }

        private async Task CurrentGame_GameUpdated(Game game)
        {
            await _hubContext.Clients.All.UpdateGame(game);
            Thread.Sleep(100);
        }


        private async Task CurrentGame_Dealing(Player player)
        {
            await _hubContext.Clients.All.UpdatePlayer(player);

        }

        private async Task CurrentGame_Shuffled(Game game)
        {
            await _hubContext.Clients.All.UpdateGame(game);
        }

        private async Task CurrentGame_GameInitialized(Game game)
        {
            await _hubContext.Clients.All.UpdatePlayers(game.Players);
            await _hubContext.Clients.All.UpdateGame(game);
        }

        public Room(string name, IHubContext<GameHub, IGameHub> hubContext)
        {
            _hubContext = hubContext;
            Id = Guid.NewGuid();
            Name = name;
            Players = new List<Player>();
        }

        public Player AddPlayer(Player player)
        {
            if (!Players.Any(it => it.Equals(player)))
                Players.Add(player);
            else
                player = Players.FirstOrDefault(it => it.Equals(player));
            return player;
        }

        public IEnumerable<Player> GetAllPlayer()
        {
            return Players;
        }

        public Player GetPlayerByName(string name)
        {
            return Players.FirstOrDefault(it => it.Name == name);
        }




        public void RemovePlayer(Player player)
        {
            if (Players.Any(it => it.Equals(player)))
                Players.Remove(player);
        }

        public bool RemovePlayerByName(string name)
        {
            var player = Players.FirstOrDefault(it => it.Name == name);
            if (player != null)
            {
                Players.Remove(player);
                return true;
            }
            return false;
        }
    }
}
