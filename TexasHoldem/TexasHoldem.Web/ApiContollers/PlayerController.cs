using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TexasHoldem.Model;
using TexasHoldem.Web.Services;

namespace TexasHoldem.Web.ApiContoller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController: ControllerBase
    {
        private IRoomService _roomService;
        public PlayerController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IEnumerable<Player> Get()
        {
            return _roomService.Rooms[0].GetAllPlayer();
        }


        [HttpGet("name")]
        public Player Get(string name)
        {
            return _roomService.Rooms[0].GetPlayerByName(name);
        }
    }
}
