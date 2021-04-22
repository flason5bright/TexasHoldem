using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TexasHoldem.Web.Services;

namespace TexasHoldem.Web.ApiContoller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IEnumerable<Room> Get()
        {
            return _roomService.Rooms;
        }

        [HttpGet("name")]
        public Room Get(string name)
        {
            //return _roomService.Rooms.FirstOrDefault(it => it.Name == name);          
            return _roomService.Rooms.FirstOrDefault();
        }
    }
}
