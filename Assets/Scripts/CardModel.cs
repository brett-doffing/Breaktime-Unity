using System.Collections;
using System.Collections.Generic;

public class CardModel
{
    /// Prime numbers used to differentiate card rank for `intAnalyzerID` and `CCPokerAnalyzer` - (deuce=2,trey=3,four=5,five=7,...,ace=41)
    int[] kPrimes = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41};

    public bool isFaceUp = false;
    public int rank;
    public int suit;
    //   An integer is made up of four bytes.  The high-order
    //   bytes are used to hold the rank bit pattern, whereas
    //   the low-order bytes hold the suit/rank/prime value
    //   of the card.
    //
    //   +--------+--------+--------+--------+
    //   |xxxbbbbb|bbbbbbbb|cdhsrrrr|xxpppppp|
    //   +--------+--------+--------+--------+
    //
    //   p = prime number of rank (deuce=2,trey=3,four=5,five=7,...,ace=41)
    //   r = rank of card (deuce=0,trey=1,four=2,five=3,...,ace=12)
    //   cdhs = suit of card
    //   b = bit turned on depending on rank of card
    public int id {
        get {
            if (rank == 1) {
                // If the card is an Ace
                switch (suit) {
                case 1: // CLUBS
                    return kPrimes[12] | (12 << 8) | 0x8000 | (1 << (16+12));
                case 2: // DIAMONDS
                    return kPrimes[12] | (12 << 8) | 0x4000 | (1 << (16+12));
                case 3: // HEARTS
                    return kPrimes[12] | (12 << 8) | 0x2000 | (1 << (16+12));
                case 4: // SPADES
                    return kPrimes[12] | (12 << 8) | 0x1000 | (1 << (16+12));
                default:
                    return 0;
                }
            } else {
                switch (suit) {
                case 1: // CLUBS
                    return kPrimes[rank-2] | ((rank-2) << 8) | 0x8000 | (1 << (16+(rank-2)));
                case 2: // DIAMONDS
                    return kPrimes[rank-2] | ((rank-2) << 8) | 0x4000 | (1 << (16+(rank-2)));
                case 3: // HEARTS
                    return kPrimes[rank-2] | ((rank-2) << 8) | 0x2000 | (1 << (16+(rank-2)));
                case 4: // SPADES
                    return kPrimes[rank-2] | ((rank-2) << 8) | 0x1000 | (1 << (16+(rank-2)));
                default:
                    return 0;
                }
            }
        }
    }

    public CardModel(int rank, int suit) {
        this.rank = rank;
        this.suit = suit;
    }
}
