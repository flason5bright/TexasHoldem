using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TexasHoldem.Model;

namespace TexasHoldem.Web.Services
{
    public class PokerService
    {
        public static PokerService Instance = new PokerService();

        public IList<Poker> AllPokers { get; private set; }


        private PokerService()
        {
            InitailizePokers();
        }
        private void InitailizePokers()
        {
            AllPokers = new List<Poker>();
            int index = 0;
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (PokerSize size in Enum.GetValues(typeof(PokerSize)))
                {
                    AllPokers.Add(new Poker(index++, size, suit));
                }
            }
        }
        public void Shuffle()
        {
            Random random = new Random();
            int tempIndex = 0;
            Poker temp = null;
            for (int i = 0; i < 52; i++)
            {
                tempIndex = random.Next(52);
                temp = AllPokers[tempIndex];
                AllPokers[tempIndex] = AllPokers[i];
                AllPokers[i] = temp;
            }
        }
    }
}
