using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool isFaceUp = true;
    public int rank = 1;
    public int suit = 1;

    public GameObject front;
    public GameObject back;

    public void setProperties(int _rank, int _suit) {
        rank = _rank;
        suit = _suit;

        if (rank >= 10) {value = 10;} 
        else {value = rank;}

        string cardName = string.Format("{0}_{1}", rank, suit);
        Texture frontTexture = Resources.Load<Texture>(cardName);
        Texture backTexture = Resources.Load<Texture>("backOfCard");
        // Texture normalTexture = Resources.Load<Texture>("fabric");
        // front.GetComponent<Renderer>().material.SetTexture("_MainTex", frontTexture);
        // front.GetComponent<Renderer>().material.EnableKeyword ("_NORMALMAP");
        // front.GetComponent<Renderer>().material.SetTexture("_BumpMap", normalTexture);
        // back.GetComponent<Renderer>().material.SetTexture("_MainTex", backTexture);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        if (Input.GetMouseButtonDown(0))
        {
            // Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    // Debug.Log(hit.collider.CompareTag);
                    Card zeecard = hit.collider.GetComponent<Card>();
                    if (zeecard != null) {
                        // Rotate
                        if (isFaceUp) {
                            iTween.RotateTo(
                                this.gameObject,
                                iTween.Hash(
                                    "rotation", new Vector3(0, 0, 180),
                                    "time", 0.1f,
                                    "easetype", iTween.EaseType.linear
                                )
                            );
                        } else {
                            iTween.RotateTo(
                                this.gameObject,
                                iTween.Hash(
                                    "rotation", new Vector3(0, 0, -360),
                                    "time", 0.1f,
                                    "easetype", iTween.EaseType.linear
                                )
                            );
                        }
                        isFaceUp = !isFaceUp;
                    }
                    
                }
            }
        }
    }
}
