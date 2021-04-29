using System;
using System.Collections.Generic;
using System.Linq;
using TexasHoldem.Model;

namespace TexasHoldem.Web.Services
{
    public class FinalPoker
    {
        public List<Poker> Pokers { get; }

        public float Result { get; set; }

        public string PlayerName { get; set; }

        public string Style
        {
            get
            {
                if (Result >= 9)
                    return PokerStyle.同花顺.ToString();
                else if (Result > 8)
                    return PokerStyle.四条.ToString();
                else if (Result > 7)
                    return PokerStyle.葫芦.ToString();
                else if (Result > 6)
                    return PokerStyle.同花.ToString();
                else if (Result > 5)
                    return PokerStyle.顺子.ToString();
                else if (Result > 4)
                    return PokerStyle.三条.ToString();
                else if (Result > 3)
                    return PokerStyle.两对.ToString();
                else if (Result > 2)
                    return PokerStyle.一对.ToString();
                else if (Result > 1)
                    return PokerStyle.高牌.ToString();
                else
                    return PokerStyle.What.ToString();
            }

        }

        public FinalPoker(Poker poker1, Poker poker2, IList<Poker> riverPoker, string name)
        {
            this.PlayerName = name;
            Pokers = new List<Poker>();
            Pokers.Add(poker1);
            Pokers.Add(poker2);
            Pokers.AddRange(riverPoker);
            Pokers = Pokers.OrderBy(it => it.PokerSize).ToList();
        }
    }

    public enum PokerStyle
    {
        What = 0,
        高牌 = 1,
        一对,
        两对,
        三条,
        顺子,
        同花,
        葫芦,
        四条,
        同花顺
    }
}
