using System.Collections;
using System.Collections.Generic;

public class HandModel 
{
    public List<int> cardIDs;
    public bool handWon = false;
    public int analyzerNum = 0;
    public int handRank = 0;

    public HandModel(List<int> cardIDs) {
        this.cardIDs = cardIDs;
    }

    public string getHandRankString() {
        if (this.analyzerNum > 6185) {return "High Card";}          //  1277 high card
        if (this.analyzerNum > 3325) {return "Pair";}              //  2860 one pair
        if (this.analyzerNum > 2467) {return "Two Pair";}           //  858 two pair
        if (this.analyzerNum > 1609) {return "Three-of-a-Kind";}               //  858 three-kind
        if (this.analyzerNum > 1599) {return "Straight";}          //  10 straights
        if (this.analyzerNum > 322)  {return "Flush";}             //  1277 flushes
        if (this.analyzerNum > 166)  {return "Full House";}         //  156 full house
        if (this.analyzerNum > 10)   {return "Four-of-a-Kind";}             //  156 four-kind
        if (this.analyzerNum > 1)    {return "Straight Flush";}     //  10 straight-flushes
        return "Royal Flush";                                    //  4 royal-flushes
    }
}
