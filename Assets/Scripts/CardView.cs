using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardView : MonoBehaviour
{
    public GameObject front;
    public GameObject back;
    public CardController controller;

    public void setupWith(CardController controller) {
        this.controller = controller;
        setTextures();
    }

    public void setTextures() {
        Texture[] textures = controller.getTextures();
        // Debug.Log(textures[0], textures[1]);
        Texture frontTexture = textures[0];
        Texture backTexture = textures[1];
        front.GetComponent<Renderer>().material.SetTexture("_MainTex", frontTexture);
        back.GetComponent<Renderer>().material.SetTexture("_MainTex", backTexture);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
