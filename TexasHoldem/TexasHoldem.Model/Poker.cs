using System;

namespace TexasHoldem.Model
{
    public class Poker
    {
        public int Id { get; set; }
        public PokerSize PorkerSize { get; set; }

        public bool IsBack { get; set; } = true;
        public string BackImg { get { return "/img/pokers/54.png"; } }
        public Suit Suit { get; set; }
        public string Img { get; }

        public Poker(int id, PokerSize size, Suit suit)
        {
            Id = id;
            PorkerSize = size;
            Suit = suit;
            Img = $"/img/pokers/{id}.png";
        }
    }
}
