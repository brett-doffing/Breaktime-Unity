using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomCard : MonoBehaviour
{
    public GameObject topPip;
    public GameObject bottomPip;
    public TextMeshPro topText;
    public TextMeshPro bottomText;
    public CardController controller;
    public GameObject backOfCard;

    public void setupWith(CardController controller) {
        this.controller = controller;
        setupAppearance();
    }

    public void setupAppearance() {
        string suit;
        if (controller.model.suit == 1)
            suit = "club";
        else if (controller.model.suit == 2)
            suit = "diamond";
        else if (controller.model.suit == 3)
            suit = "heart";
        else
            suit = "spade";

        Texture pipTexture = Resources.Load<Texture>(string.Format("Images/{0}", suit));
        topPip.GetComponent<Renderer>().material.SetTexture("_MainTex", pipTexture);
        bottomPip.GetComponent<Renderer>().material.SetTexture("_MainTex", pipTexture);
        Texture backTexture = Resources.Load<Texture>("Images/backOfCard");
        backOfCard.GetComponent<Renderer>().material.SetTexture("_MainTex", backTexture);

        string rank;
        switch (controller.model.rank)
        {
            case 1:
                rank = "A";
                break;
            case 13:
                rank = "K";
                break;
            case 12:
                rank = "Q";
                break;
            case 11:
                rank = "J";
                break;
            case 10:
                rank = "T";
                break;
            default:
                rank = controller.model.rank.ToString();
                break;
        }
        topText.text = rank;
        bottomText.text = rank;

        if (controller.model.suit == 2 || controller.model.suit == 3) {
            topText.color = new Color32(255, 0, 0, 255);
        } else {
            topText.color = new Color32(0, 0, 0, 255);
        }
    }
}
