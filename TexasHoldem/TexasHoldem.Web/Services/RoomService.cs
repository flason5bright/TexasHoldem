using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TexasHoldem.Model;
using TexasHoldem.Web.Hubs;

namespace TexasHoldem.Web.Services
{
    public interface IRoomService
    {
        IList<Room> Rooms { get; set; }

        bool CreateRoom(string name);

        void DeleteRoom(Guid id);

        Room FindRoom(Guid id);
    }

    public class RoomService : IRoomService
    {
        public IList<Room> Rooms { get; set; } = new List<Room>();
        private readonly IHubContext<GameHub, IGameHub> _hubContext;
        public RoomService(IHubContext<GameHub, IGameHub> hubContext)
        {
            _hubContext = hubContext;
            CreateRoom("DE_FUN");
        }

        public bool CreateRoom(string name)
        {
            Rooms.Add(new Room(name, _hubContext));
            return true;
        }

        public void DeleteRoom(Guid id)
        {
            var room = Rooms.FirstOrDefault(it => it.Id == id);
            if (room != null)
            {
                Rooms.Remove(room);
            }
        }

        public Room FindRoom(Guid id)
        {
            return Rooms.FirstOrDefault(it => it.Id == id);
        }

    }
}
