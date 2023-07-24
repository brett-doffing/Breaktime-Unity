using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    List<GameObject> cards = new List<GameObject>();
    public GameObject cardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 13; i++) {
            for (int j = 1; j <= 4; j++) {
                GameObject newCard = Instantiate(cardPrefab, new Vector3(i * j * 0.5f, i * j * 0.01f, 0), Quaternion.Euler(0f, 0f, 180f));
                newCard.gameObject.GetComponent<CardView>().viewModel.setProperties(i, j);
                // shoe.cards.Add(newCard);
            }
        }
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
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);
                    // Debug.Log(hit.collider.CompareTag);
                    CardView zeecard = hit.collider.GetComponent<CardView>();
                    if (zeecard != null) {
                        // Rotate
                        if (zeecard.viewModel.model.isFaceUp != true) {
                            iTween.RotateTo(
                                zeecard.gameObject,
                                iTween.Hash(
                                    "rotation", new Vector3(0, 0, 180),
                                    "time", 0.1f,
                                    "easetype", iTween.EaseType.linear
                                )
                            );
                        } else {
                            iTween.RotateTo(
                                zeecard.gameObject,
                                iTween.Hash(
                                    "rotation", new Vector3(0, 0, -360),
                                    "time", 0.1f,
                                    "easetype", iTween.EaseType.linear
                                )
                            );
                        }
                        zeecard.viewModel.model.isFaceUp = !zeecard.viewModel.model.isFaceUp;
                    }
                }
            }
        }
    }
}
