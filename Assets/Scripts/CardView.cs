using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardView : MonoBehaviour
{
    public GameObject front;
    public GameObject back;
    public CardVM viewModel;

    public void initVM(CardVM viewModel) {
        this.viewModel = viewModel;
        setTextures();
    }

    public void setTextures() {
        Texture[] textures = viewModel.getTextures();
        Debug.Log(textures[0], textures[1]);
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
