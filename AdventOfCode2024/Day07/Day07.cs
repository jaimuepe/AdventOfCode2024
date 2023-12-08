using AdventOfCode2024.Core;

namespace AdventOfCode2024.Day07;

public class Day07 : DayX
{
    private static readonly int[] ConversionTable = new int ['T' + 1];

    static Day07()
    {
        ConversionTable['A'] = 13;
        ConversionTable['K'] = 12;
        ConversionTable['Q'] = 11;
        ConversionTable['J'] = 10;
        ConversionTable['T'] = 9;
        ConversionTable['9'] = 8;
        ConversionTable['8'] = 7;
        ConversionTable['7'] = 6;
        ConversionTable['6'] = 5;
        ConversionTable['5'] = 4;
        ConversionTable['4'] = 3;
        ConversionTable['3'] = 2;
        ConversionTable['2'] = 1;
        ConversionTable['*'] = 0;
    }

    private enum eHandType
    {
        FiveOfAKind,
        FourOfAKind,
        FullHouse,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard,
    }

    private class Hand : IComparable<Hand>
    {
        public string RawCards { get; }

        public int[] Cards { get; }

        public long Bid { get; }

        public eHandType HandType { get; }

        public Hand(string rawCards, int[] cards, long bid)
        {
            RawCards = rawCards;
            Cards = cards;
            Bid = bid;

            HandType = CalculateHandType();
        }


        private eHandType CalculateHandType()
        {
            var hits = new int[ConversionTable['A'] + 1];

            foreach (var card in Cards) hits[ConversionTable[card]]++;

            var anyFourOfAKind = false;
            var anyThreeOfAKind = false;
            var numberOfPairs = 0;

            var wildcards = hits[0];

            for (var i = 1; i < hits.Length; i++)
            {
                var hit = hits[i];

                var hitPlusWildcards = hit + wildcards;

                if (hitPlusWildcards == 5) return eHandType.FiveOfAKind;

                if (hitPlusWildcards == 4) anyFourOfAKind = true;
                else if (hit == 3) anyThreeOfAKind = true;
                else if (hit == 2) numberOfPairs++;
            }

            if (anyFourOfAKind) return eHandType.FourOfAKind;

            // trio + pair || pair + pair + wildcard
            if (anyThreeOfAKind && numberOfPairs == 1 || numberOfPairs == 2 && wildcards == 1)
                return eHandType.FullHouse;

            // trio || pair + wildcard || single + wildcard + wildcard
            if (anyThreeOfAKind || numberOfPairs == 1 && wildcards == 1 || wildcards == 2)
                return eHandType.ThreeOfAKind;

            if (numberOfPairs == 2) return eHandType.TwoPair;

            // pair || single + wildcard
            if (numberOfPairs == 1 || wildcards == 1) return eHandType.OnePair;

            return eHandType.HighCard;
        }

        public int CompareTo(Hand? other)
        {
            var thisType = HandType;
            var otherType = other!.HandType;

            if (thisType != otherType) return thisType.CompareTo(otherType);

            for (var i = 0; i < Cards.Length; i++)
            {
                if (Cards[i] != other.Cards[i])
                {
                    // descending
                    return ConversionTable[other.Cards[i]].CompareTo(ConversionTable[Cards[i]]);
                }
            }

            return 0;
        }

        public override string ToString()
        {
            return RawCards + " - " + HandType;
        }
    }

    protected override string Test => "32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483";

    protected override void SolvePart1(List<string> lines)
    {
        SolveCommon(ParseHands(lines));
    }

    protected override void SolvePart2(List<string> lines)
    {
        SolveCommon(ParseHands(lines));
    }

    private static void SolveCommon(IEnumerable<Hand> hands)
    {
        var handsSorted = hands.Order().ToArray();

        var result = 0L;

        for (var i = 0; i < handsSorted.Length; i++)
        {
            var hand = handsSorted[i];

            var rank = handsSorted.Length - i;

            result += hand.Bid * rank;
        }

        PrintResult(result);
    }

    private IEnumerable<Hand> ParseHands(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var cardsToken = line[..5];
            var bidToken = line[6..];

            var cards = new int[5];
            for (var i = 0; i < cardsToken.Length; i++)
            {
                var card = cardsToken[i];
                if (IsPart2 && card == 'J') card = '*';

                cards[i] = card;
            }

            yield return new Hand(cardsToken, cards, long.Parse(bidToken));
        }
    }
}