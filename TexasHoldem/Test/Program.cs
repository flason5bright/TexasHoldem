using System;
using System.Collections.Generic;
using System.Reflection;
using TexasHoldem.Model;
using TexasHoldem.Web.Services;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            PokerService.Instance.Shuffle();
            var pokers = PokerService.Instance.AllPokers;
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine($"{pokers[i].Suit} : {pokers[i].Value}");
            }

            List<Poker> river = new List<Poker>() { pokers[2], pokers[3], pokers[4], pokers[5], pokers[6] };

            FinalPoker fPoker = new FinalPoker(pokers[0], pokers[1], river,"");
            Console.WriteLine(PokerService.Instance.Compute(fPoker));
            Console.ReadLine();
        }
    }
}
