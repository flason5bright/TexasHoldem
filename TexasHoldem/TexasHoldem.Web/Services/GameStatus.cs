using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TexasHoldem.Model;

namespace TexasHoldem.Web.Services
{

    public enum GameStatus
    {
        NotStarted = 0,
        Started,
        Playing,
        Finished
    }



}
