using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TexasHoldem.Model;
using TexasHoldem.Web.Services;

namespace TexasHoldem.Web.Hubs
{
    public interface IGameHub
    {
        Task AddPlayer(Player player);
        Task RemovePlayer(Player player);

        Task UpdatePlayer(Player player);
        Task UpdatePlayers(IEnumerable<Player> players);
        Task UpdateGame(Game game);

        Task StartGame();

        Task Bet(int bet);

        Task Check();

    }

    public class GameHub : Hub<IGameHub>, IGameHub
    {
        private IRoomService _roomService;
        private IHttpContextAccessor _httpContextAccessor;

        public Player Player
        {
            get
            {
                var claims = (_httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity).Claims.ToList();
                var name = claims[0].Value;
                _roomService.Rooms.First().GetPlayerByName(name);
                return _roomService.Rooms.First().GetPlayerByName(name); ;
            }
        }

        private static Dictionary<string, Player> _playerDic = new Dictionary<string, Player>();

        public GameHub(IRoomService roomService, IHttpContextAccessor httpContextAccessor)
        {
            _roomService = roomService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task AddPlayer(Player player)
        {
            await Clients.All.AddPlayer(player);
        }
        public async Task RemovePlayer(Player player)
        {
            _roomService.Rooms.First().RemovePlayer(player);
            await Clients.All.RemovePlayer(player);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            //var claims = (_httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity).Claims.ToList();
            //var name = claims[0].Value;
            //_roomService.Rooms.First().RemovePlayerByName(name);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(string roomName, Guid id)
        {
            var _room = _roomService.FindRoom(id);
            //_room.AddPlayer(_player);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            //notify all player in the room
            //await Clients.Group(_room.Name).SendAsync("OnPlayerJoinRoom", JsonConvert.SerializeObject(_player));

        }

        public async Task<bool> SelectSeat(int index)
        {
            if (_roomService.Rooms.First().Players.Any(it => it.Index == index))
                return false;
            Player.Index = index;
            if (Player.PlayerStatus != PlayerStatus.OnSeat)
                Player.PlayerStatus = PlayerStatus.OnSeat;
            await UpdatePlayer(Player);
            return true;
        }

        public async Task UpdatePlayer(Player player)
        {
            await Clients.All.UpdatePlayer(player);
        }

        public async Task StartGame()
        {
            var room = _roomService.Rooms.First();
            await room.StartNewGame();

        }

        public async Task UpdatePlayers(IEnumerable<Player> players)
        {
            await Clients.All.UpdatePlayers(players);
        }

        public async Task UpdateGame(Game game)
        {
            await Clients.All.UpdateGame(game);
        }

        public async Task Bet(int bet)
        {
            var room = _roomService.Rooms.First();

            Player.BetMoney = bet;
            Player.Money -= bet;

            Player.IsActive = false;
            Player.IsCheck = false;
            Player.CurrentRound = room.CurrentGame.CurrentRound;
            await Clients.All.UpdatePlayer(Player);

            room.CurrentGame.MaxBet = Player.BetMoney;
            room.CurrentGame.SetPoolChips();

            Thread.Sleep(500);
            await Clients.All.UpdateGame(room.CurrentGame);
        }

        public async Task Check()
        {
            var room = _roomService.Rooms.First();
            Player.IsActive = false;
            Player.IsCheck = true;
            Player.CurrentRound = room.CurrentGame.CurrentRound;
            await Clients.All.UpdatePlayer(Player);

           
            room.CurrentGame.IsAllCheck();
            Thread.Sleep(500);
            await Clients.All.UpdateGame(room.CurrentGame);

        }
        public async Task Fold()
        {
            Player.IsActive = false;
            Player.IsCheck = false;

            Player.Status = GamePlayerStatus.Fold;
            await Clients.All.UpdatePlayer(Player);
            var room = _roomService.Rooms.First();
            room.CurrentGame.Next();
            Thread.Sleep(500);
            await Clients.All.UpdateGame(room.CurrentGame);

        }
    }
}
