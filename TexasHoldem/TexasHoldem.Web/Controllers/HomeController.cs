using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TexasHoldem.Web.Models;
using TexasHoldem.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using TexasHoldem.Web.Hubs;

namespace TexasHoldem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRoomService _roomService;
        private readonly IHubContext<GameHub, IGameHub> _hubContext;
        public HomeController(ILogger<HomeController> logger, IRoomService roomService, IHubContext<GameHub, IGameHub> hubContext)
        {
            _logger = logger;
            _roomService = roomService;
            _hubContext = hubContext;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        [Authorize]
        public IActionResult Home()
        {
            return View();
        }

        [Authorize]
        public IActionResult Room()
        {
            ViewData["Rooms"] = _roomService.Rooms;
            return View();
        }

        [Authorize]
        public IActionResult Index(string id)
        {
            var room = _roomService.FindRoom(new Guid(id));
            var claims = (HttpContext.User.Identity as ClaimsIdentity).Claims.ToList();
            var name = claims[0].Value;
            var avatar = claims[1].Value;
            var player = new Model.Player(name, avatar);
            var realPlayer = room.AddPlayer(player);
            ViewBag.Player = realPlayer;
            if (player.Id == realPlayer.Id)
                _hubContext.Clients.All.AddPlayer(player);
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(string name, string avatar, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;

            var userClaims = new List<Claim>()
            {
                new Claim("name", name),
                new Claim("avatar", avatar)
            };
            var identity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(new[] { identity });
            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),//有效时间
                    AllowRefresh = true
                }).Wait();

            return RedirectToLocal(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //内部跳转
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {//如果是本地
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Room), "Home");
        }
    }
}
