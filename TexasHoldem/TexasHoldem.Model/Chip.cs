namespace TexasHoldem.Model
{
    public class Chip
    {
        public int Money { get; set; }

        public int Num { get; set; }
        public Chip(int money, int num)
        {
            this.Money = money;
            this.Num = num;

        }

    }
}