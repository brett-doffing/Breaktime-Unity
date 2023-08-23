using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CardEvent : UnityEvent<int> {}

public class CardContainer : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    public int id;
    [SerializeField] public CardEvent onTouch;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.LogFormat("Deck Count {0}", cards.Count);
    }

    // Update is called once per frame
    void Update() {}

    void OnMouseDown() 
    {
        onTouch?.Invoke(id);
    }

    public void addToHand(GameObject card) {
        cards.Add(card);
        card.transform.position = new Vector3(
            transform.position.x,
            0.1f + (cards.Count * 0.01f),
            transform.position.z + zOffset(cards.Count)
        );
    }

    public HandModel getHandModel() {
        List<int> cardIDs = cards.Select(card => card.GetComponent<CardView>().controller.model.id).ToList();
        return new HandModel(cardIDs);
    }

    int zOffset(int position) {
        switch (position) {
            case 1:
                return 10;
            case 2:
                return 5;
            case 3:
                return 0;
            case 4:
                return -5;
            case 5:
                return -10;
            default:
                return 0;
        }
    }
}
