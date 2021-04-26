using System;

namespace TexasHoldem.Model
{
    public class Poker
    {
        public int Id { get; set; }
        public PokerSize PokerSize { get; set; }

        public bool IsBack { get; set; } = true;
        public string BackImg { get { return "/img/pokers/54.png"; } }
        public Suit Suit { get; set; }
        public string Img { get; }

        public string Value { get { return ((int)this.PokerSize).ToString("00"); } }

        public Poker(int id, PokerSize size = PokerSize.Default, Suit suit = Suit.Default)
        {
            Id = id;
            if (size == PokerSize.Ace)
                PokerSize = PokerSize.Max;
            else
                PokerSize = size;
            Suit = suit;
            Img = $"/img/pokers/{id}.png";
        }

        public Poker(Poker poker)
        {
            Id = poker.Id;
            PokerSize = PokerSize.Ace;
            Suit = poker.Suit;
            Img = $"/img/pokers/{Id}.png";
        }
    }
}
