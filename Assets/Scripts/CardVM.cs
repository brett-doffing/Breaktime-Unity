using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVM
{
    public CardModel model = new CardModel();

    public void setProperties(int _rank, int _suit) {
        model.rank = _rank;
        model.suit = _suit;

        string cardName = string.Format("{0}_{1}", rank, suit);
        Texture frontTexture = Resources.Load<Texture>(cardName);
        // Texture backTexture = Resources.Load<Texture>("backOfCard");
        // Texture normalTexture = Resources.Load<Texture>("fabric");
        front.GetComponent<Renderer>().material.SetTexture("_MainTex", frontTexture);
        // front.GetComponent<Renderer>().material.EnableKeyword ("_NORMALMAP");
        // front.GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
        back.GetComponent<Renderer>().material.SetTexture("_MainTex", backTexture);
    }
}
