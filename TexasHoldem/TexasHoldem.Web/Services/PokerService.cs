using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
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
                if (suit == Suit.Default)
                    continue;
                foreach (PokerSize size in Enum.GetValues(typeof(PokerSize)))
                {
                    if (size == PokerSize.Max || size == PokerSize.Default)
                        continue;
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

        public IEnumerable<FinalPoker> Summary(List<FinalPoker> finalPokers)
        {
            foreach (var item in finalPokers)
            {
                var weight = Compute(item);
                var result = float.Parse(weight);
                item.Result = result;
            }
            return finalPokers.OrderByDescending(it => it.Result);
        }

        public string Compute(FinalPoker finalPoker)
        {
            int sum_S = finalPoker.Pokers.Count(it => it.Suit == Suit.Spade);
            int sum_H = finalPoker.Pokers.Count(it => it.Suit == Suit.Heart);
            int sum_D = finalPoker.Pokers.Count(it => it.Suit == Suit.Diamond);
            int sum_C = finalPoker.Pokers.Count(it => it.Suit == Suit.Club);

            var singleCards = finalPoker.Pokers.OrderBy(it => it.PokerSize).GroupBy(it => it.PokerSize).Select(it => new SingleCard(it.Key, it.Count())).ToList();

            try
            {

                if (singleCards.Count < 5)
                {
                    //判断4条
                    var single = singleCards.FirstOrDefault(it => it.Count == 4);
                    if (single != null)
                    {
                        var index = singleCards.IndexOf(single);
                        SingleCard another;
                        if (index == singleCards.Count - 1)
                            another = singleCards[index - 1];
                        else
                            another = singleCards.Last();
                        return "8." + another.Value;
                    }

                    //三带二
                    single = singleCards.LastOrDefault(it => it.Count == 3);
                    if (single != null)
                    {
                        SingleCard another = singleCards.LastOrDefault(it => it.Count >= 2 && it.PokerSize != single.PokerSize);
                        if (another != null)
                            return "7." + another.Value;
                    }

                }
                else
                {
                    //同花
                    if (sum_S >= 5)
                    {
                        //同花顺
                        var index = JudgeStraightWithColor(finalPoker.Pokers.Where(it => it.Suit == Suit.Spade).ToList());
                        if (index > 0)
                            return "9." + index;
                        var pokers = finalPoker.Pokers.Where(it => it.Suit == Suit.Spade).TakeLast(5).ToList();
                        return "6." + pokers[4].Value + pokers[3].Value + pokers[2].Value + pokers[1].Value + pokers[0].Value;
                    }

                    if (sum_H >= 5)
                    {
                        //同花顺
                        var index = JudgeStraightWithColor(finalPoker.Pokers.Where(it => it.Suit == Suit.Heart).ToList());
                        if (index > 0)
                            return "9." + index;
                        var pokers = finalPoker.Pokers.Where(it => it.Suit == Suit.Heart).TakeLast(5).ToList();
                        return "6." + pokers[4].Value + pokers[3].Value + pokers[2].Value + pokers[1].Value + pokers[0].Value;
                    }

                    if (sum_D >= 5)
                    {
                        //同花顺
                        var index = JudgeStraightWithColor(finalPoker.Pokers.Where(it => it.Suit == Suit.Diamond).ToList());
                        if (index > 0)
                            return "9." + index;
                        var pokers = finalPoker.Pokers.Where(it => it.Suit == Suit.Diamond).TakeLast(5).ToList();
                        return "6." + pokers[4].Value + pokers[3].Value + pokers[2].Value + pokers[1].Value + pokers[0].Value;
                    }

                    if (sum_C >= 5)
                    {
                        //同花顺
                        var index = JudgeStraightWithColor(finalPoker.Pokers.Where(it => it.Suit == Suit.Club).ToList());
                        if (index > 0)
                            return "9." + index;
                        var pokers = finalPoker.Pokers.Where(it => it.Suit == Suit.Club).TakeLast(5).ToList();
                        return "6." + pokers[4].Value + pokers[3].Value + pokers[2].Value + pokers[1].Value + pokers[0].Value;
                    }

                    //顺子
                    if (singleCards.Count >= 5)
                    {
                        var index = JudgeStraight(singleCards.ToList());
                        if (index > 0)
                            return "5." + index;
                    }

                    //三条
                    var single = singleCards.LastOrDefault(it => it.Count == 3);
                    if (single != null)
                    {

                        var another = singleCards.LastOrDefault(it => it.PokerSize != single.PokerSize);
                        var another2 = singleCards.LastOrDefault(it => it.PokerSize != single.PokerSize && it.PokerSize != another.PokerSize);
                        return "4." + another.Value + another2.Value;
                    }

                    //两对
                    if (singleCards.Count == 5)
                    {
                        single = singleCards.LastOrDefault(it => it.Count == 2);
                        var another = singleCards.LastOrDefault(it => it.Count == 2 && it.PokerSize != single.PokerSize);
                        var another2 = singleCards.LastOrDefault(it => it.Count == 1);
                        return "3." + single.Value + another.Value + another2.Value;
                    }
                    //一对
                    if (singleCards.Count == 6)
                    {
                        single = singleCards.LastOrDefault(it => it.Count == 2);
                        var another = singleCards.LastOrDefault(it => it.Count == 1);
                        var another2 = singleCards.LastOrDefault(it => it.Count == 1 && it.PokerSize != another.PokerSize);
                        var another3 = singleCards.LastOrDefault(it => it.Count == 1 && it.PokerSize != another.PokerSize && it.PokerSize != another2.PokerSize);
                        return "2." + single.Value + another.Value + another2.Value + another3.Value;
                    }
                    //高牌

                    return "1." + singleCards[6].Value + singleCards[5].Value + singleCards[4].Value + singleCards[3].Value + singleCards[2].Value + singleCards[1].PokerSize + singleCards[0].Value;
                }
            }
            catch
            {
                return "0";
            }
            return "0";
        }

        private int JudgeStraight(List<SingleCard> pokers)
        {
            var count = 0;
            bool hasAce = false;
            if (pokers.Last().PokerSize == PokerSize.Max)
                pokers.Insert(0, SingleCard.GetFakeAce());
            var lastIndex = 0;

            for (int i = 0; i < pokers.Count; i++)
            {
                if (i + 1 == pokers.Count)
                    break;
                if (pokers[i].PokerSize == pokers[i + 1].PokerSize - 1)
                {
                    count++;
                    if (count >= 4)
                    {
                        if (hasAce)
                            lastIndex = i - 1;
                        else
                            lastIndex = i;
                    }
                    continue;
                }

                count = 0;
            }

            return lastIndex;

        }

        private int JudgeStraightWithColor(List<Poker> pokers)
        {
            var count = 0;
            bool hasAce = false;
            if (pokers.Last().PokerSize == PokerSize.Max)
                pokers.Insert(0, new Poker(pokers.Last()));
            var lastIndex = 0;

            for (int i = 0; i < pokers.Count; i++)
            {
                if (i + 1 == pokers.Count)
                    break;
                if (pokers[i].PokerSize == pokers[i + 1].PokerSize - 1)
                {
                    count++;
                    if (count >= 4)
                    {
                        if (hasAce)
                            lastIndex = i - 1;
                        else
                            lastIndex = i;
                    }
                    continue;
                }

                count = 0;
            }

            return lastIndex;

        }
    }

    public class SingleCard
    {
        public PokerSize PokerSize { get; set; }

        public int Count { get; set; }

        public string Value { get { return ((int)this.PokerSize).ToString("00"); } }

        public SingleCard(PokerSize pokerSize, int count)
        {
            PokerSize = pokerSize;
            Count = count;
        }

        public static SingleCard GetFakeAce()
        {
            return new SingleCard(PokerSize.Ace, 1);
        }
    }
}
