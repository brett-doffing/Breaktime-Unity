using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVM
{
    public CardModel model;

    public CardVM() {}

    public CardVM(CardModel model) {
        this.model = model;
    }

    public Texture[] getTextures() {
        string cardName = string.Format("{0}_{1}", model.rank, model.suit);
        Texture frontTexture = Resources.Load<Texture>(string.Format("Images/{0}", cardName));
        Texture backTexture = Resources.Load<Texture>("Images/backOfCard");
        return new Texture[] {frontTexture, backTexture};
    }
}
