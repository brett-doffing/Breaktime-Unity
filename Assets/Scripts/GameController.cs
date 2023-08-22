using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cardPrefab;
    public GameObject hand1;
    public GameObject hand2;
    public GameObject hand3;
    public GameObject hand4;
    public GameObject hand5;
    public GameObject oppHand1;
    public GameObject oppHand2;
    public GameObject oppHand3;
    public GameObject oppHand4;
    public GameObject oppHand5;
    public GameObject playableHand;
    public GameObject oppPlayableHand;
    public GameObject deck;
    CardContainer _hand1;
    CardContainer _hand2;
    CardContainer _hand3;
    CardContainer _hand4;
    CardContainer _hand5;
    CardContainer _oppHand1;
    CardContainer _oppHand2;
    CardContainer _oppHand3;
    CardContainer _oppHand4;
    CardContainer _oppHand5;
    CardContainer _playableHand;
    CardContainer _oppPlayableHand;
    CardContainer _deck;
    bool _canDraw = true;
    bool _isGameOn = true;
    int _playerTotalCards = 0;
    int _oppTotalCards = 0;

    // Start is called before the first frame update
    void Start()
    {
        _hand1 = hand1.gameObject.GetComponent<CardContainer>();
        _hand2 = hand2.gameObject.GetComponent<CardContainer>();
        _hand3 = hand3.gameObject.GetComponent<CardContainer>();
        _hand4 = hand4.gameObject.GetComponent<CardContainer>();
        _hand5 = hand5.gameObject.GetComponent<CardContainer>();
        _oppHand1 = oppHand1.gameObject.GetComponent<CardContainer>();
        _oppHand2 = oppHand2.gameObject.GetComponent<CardContainer>();
        _oppHand3 = oppHand3.gameObject.GetComponent<CardContainer>();
        _oppHand4 = oppHand4.gameObject.GetComponent<CardContainer>();
        _oppHand5 = oppHand5.gameObject.GetComponent<CardContainer>();
        _playableHand = playableHand.gameObject.GetComponent<CardContainer>();
        _oppPlayableHand = oppPlayableHand.gameObject.GetComponent<CardContainer>();
        _deck = deck.gameObject.GetComponent<CardContainer>();

        createDeck();
    }

    // Update is called once per frame
    // void Update() {}

    void createDeck() {
        for (int i = 1; i <= 13; i++) {
            for (int j = 1; j <= 4; j++) {
                CardModel model = new CardModel(i, j);
                CardController cardController = new CardController(model);
                GameObject newCard = Instantiate(
                        cardPrefab, 
                        new Vector3(0, 0, 0), 
                        Quaternion.Euler(0f, 0f, 180f)
                    );
                
                newCard.gameObject.GetComponent<CardView>().setupWith(cardController);
                _deck.cards.Add(newCard.gameObject);
            }
        }
        shuffleDeck();
    }

    void shuffleDeck() {
        System.Random random = new System.Random();  

        for(int j = 0; j < 3; j++) {
            for(int i = _deck.cards.Count - 1; i > 1; i--) {
                int rnd = random.Next(i + 1);  

                GameObject card = _deck.cards[rnd];  
                _deck.cards[rnd] = _deck.cards[i];  
                _deck.cards[i] = card;
            }
        }

        displayDeck();
    }

    void displayDeck() {
        for (int i = 0; i < _deck.cards.Count; i++) {
            _deck.cards[i].transform.position = new Vector3(0, i * 0.02f, 0);
        }
    }

    public void handleGameTouch(int id) {
        Debug.Log(id);
        switch (id)
        {
            case 0:
                if (_deck.cards.Count == 52) {
                    dealCards();
                } else if (_deck.cards.Count > 2 && _canDraw == true) {
                    addPlayableCard();
                } else {
                    if (_isGameOn == true)
                        resetGame();
                }
                break;
            case > 0 and <= 5:
                if (_playableHand.cards.Count == 1) {
                    movePlayableCardToHand(id);
                }
                break;
            default:
                Debug.Log("Nothing selected");
                break;
        }
    }

    void dealCards() {
        if (_deck.cards.Count == 52) {
            for (int i = 1; i <= 10; i++) {
                GameObject card = _deck.cards[_deck.cards.Count - 1];
                CardView cardComponent = card.GetComponent<CardView>();
                animateDealtCard(card, i);
                _deck.cards.Remove(card);
                cardComponent.controller.model.isFaceUp = !cardComponent.controller.model.isFaceUp;
            }
            _playerTotalCards += 5;
            _oppTotalCards += 5;
        }
        Debug.LogFormat("Hand 1 count = {0}", _hand1.cards.Count);
    }

    void addPlayableCard() {
        // Do not initially increment cards in hand
        if (_playableHand.cards.Count == 1) {
            _playerTotalCards += 1;
            _playableHand.cards.Clear();
            searchOppRow();
        }
        GameObject card = _deck.cards[_deck.cards.Count - 1];
        CardView cardComponent = card.GetComponent<CardView>();
        _deck.cards.Remove(card);
        _playableHand.cards.Add(card);
        _canDraw = false;


        animateDrawPlayableCard(card);
        
        cardComponent.controller.model.isFaceUp = !cardComponent.controller.model.isFaceUp;
    }

    void addOppPlayableCard() {
        GameObject card = _deck.cards[_deck.cards.Count - 1];
        CardView cardComponent = card.GetComponent<CardView>();
        _deck.cards.Remove(card);
        _oppPlayableHand.cards.Add(card);

        if (_oppTotalCards == 24) {
            _isGameOn = false;
            searchOppRow();
            StartCoroutine(performEndgame());
            performEndgame();
        } else {
            animateDrawOppPlayableCard(card);
        }
    }

    void animateDealtCard(GameObject card, int num) {
        GameObject handObject;
        CardContainer handComponent;
        switch (num)
        {
            case 1:
                handObject = hand1;
                handComponent = _hand1;
                break;
            case 2:
                handObject = oppHand1;
                handComponent = _oppHand1;
                break;
            case 3:
                handObject = hand2;
                handComponent = _hand2;
                break;
            case 4:
                handObject = oppHand2;
                handComponent = _oppHand2;
                break;
            case 5:
                handObject = hand3;
                handComponent = _hand3;
                break;
            case 6:
                handObject = oppHand3;
                handComponent = _oppHand3;
                break;
            case 7:
                handObject = hand4;
                handComponent = _hand4;
                break;
            case 8:
                handObject = oppHand4;
                handComponent = _oppHand4;
                break;
            case 9:
                handObject = hand5;
                handComponent = _hand5;
                break;
            case 10:
                handObject = oppHand5;
                handComponent = _oppHand5;
                break;
            default:
                handObject = hand1;
                handComponent = _hand1;
                break;
        }
        iTween.RotateTo(
            card.gameObject,
            iTween.Hash(
                "rotation", new Vector3(0, 0, -360),
                "time", 0.1f,
                "delay", 0.1f * num,
                "easetype", iTween.EaseType.linear
            )
        );
        iTween.MoveTo(
            card.gameObject,
            iTween.Hash(
                "position", new Vector3(
                    handObject.transform.position.x, 
                    0.1f, 
                    handObject.transform.position.z + zOffset(1)
                ),
                "time", 0.1f,
                "delay", 0.1f * num,
                "easetype", iTween.EaseType.linear
            )
        );
        handComponent.cards.Add(card);
    }

    void animateDrawPlayableCard(GameObject card) {
        iTween.RotateTo(
            card.gameObject,
            iTween.Hash(
                "rotation", new Vector3(0, 0, -360),
                "time", 0.1f,
                "easetype", iTween.EaseType.linear
            )
        );
        iTween.MoveTo(
            card.gameObject,
            iTween.Hash(
                "position", new Vector3(playableHand.transform.position.x, 0.1f, 0),
                "time", 0.1f,
                "easetype", iTween.EaseType.linear
            )
        );
    }

    void animateDrawOppPlayableCard(GameObject card) {
        iTween.MoveTo(
            card.gameObject,
            iTween.Hash(
                "position", new Vector3(oppPlayableHand.transform.position.x, 0.1f, 0),
                "time", 0.1f,
                "easetype", iTween.EaseType.linear
            )
        );
        if (_oppTotalCards < 20) {
            iTween.RotateTo(
                card.gameObject,
                iTween.Hash(
                    "rotation", new Vector3(0, 0, -360),
                    "time", 0.1f,
                    "easetype", iTween.EaseType.linear
                )
            );
        }
        
    }

    void movePlayableCardToHand(int handNumber) {
        GameObject card = _playableHand.cards[0];
        int row = _playerTotalCards / 5;
        if (_hand1.cards.Contains(card)) { _hand1.cards.Remove(card); }
        else if (_hand2.cards.Contains(card)) { _hand2.cards.Remove(card); }
        else if (_hand3.cards.Contains(card)) { _hand3.cards.Remove(card); }
        else if (_hand4.cards.Contains(card)) { _hand4.cards.Remove(card); }
        else if (_hand5.cards.Contains(card)) { _hand5.cards.Remove(card); }

        switch (handNumber)
        {
            case 1:
                if (_hand1.cards.Count == row) {
                    _hand1.addToHand(card);
                    _canDraw = true;
                }
                break;
            case 2:
                if (_hand2.cards.Count == row) {
                    _hand2.addToHand(card);
                    _canDraw = true;
                }
                break;
            case 3:
                if (_hand3.cards.Count == row) {
                    _hand3.addToHand(card);
                    _canDraw = true;
                }
                break;
            case 4:
                if (_hand4.cards.Count == row) {
                    _hand4.addToHand(card);
                    _canDraw = true;
                }
                break;
            case 5:
                if (_hand5.cards.Count == row) {
                    _hand5.addToHand(card);
                    _canDraw = true;
                }
                break;
            default:
                break;
        }

        if (_oppPlayableHand.cards.Count == 0) { addOppPlayableCard(); }
    }

    void moveOppPlayableCardToHand(int handNumber) {
        GameObject card = _oppPlayableHand.cards[0];
        // CardView cardComponent = card.GetComponent<CardView>();
        // Debug.LogFormat("{0}_{1}", cardComponent.controller.model.rank, cardComponent.controller.model.suit);
        // int handNumber = _oppTotalCards / 5;

        switch (handNumber)
        {
            case 1:
                _oppHand1.addToHand(card);
                break;
            case 2:
                _oppHand2.addToHand(card);
                break;
            case 3:
                _oppHand3.addToHand(card);
                break;
            case 4:
                _oppHand4.addToHand(card);
                break;
            case 5:
                _oppHand5.addToHand(card);
                break;
            default:
                break;
        }
        _oppTotalCards += 1;
        _oppPlayableHand.cards.Clear();
    }

    void searchOppRow() {
        int rowThreshold = _oppTotalCards / 5;
        GameObject card = _oppPlayableHand.cards[0];
        CardView cardComponent = card.GetComponent<CardView>();
        int cardRank = cardComponent.controller.model.rank;
        int handNumber = 0;

        List<int> handRanks;
        for (int i = 1; i <= 5; i++) {
            if (i == 1) {
                handRanks = _oppHand1.cards.ConvertAll(cardN => cardN.GetComponent<CardView>().controller.model.rank);
                if (handRanks.Contains(cardRank) && _oppHand1.cards.Count == rowThreshold) {
                    handNumber = 1;
                    break;
                }
            } else if (i == 2) {
                handRanks = _oppHand2.cards.ConvertAll(cardN => cardN.GetComponent<CardView>().controller.model.rank);
                if (handRanks.Contains(cardRank) && _oppHand2.cards.Count == rowThreshold) {
                    handNumber = 2;
                    break;
                }
            } else if (i == 3) {
                handRanks = _oppHand3.cards.ConvertAll(cardN => cardN.GetComponent<CardView>().controller.model.rank);
                if (handRanks.Contains(cardRank) && _oppHand3.cards.Count == rowThreshold) {
                    handNumber = 3;
                    break;
                }
            } else if (i == 4) {
                handRanks = _oppHand4.cards.ConvertAll(cardN => cardN.GetComponent<CardView>().controller.model.rank);
                if (handRanks.Contains(cardRank) && _oppHand4.cards.Count == rowThreshold) {
                    handNumber = 4;
                    break;
                }
            } else if (i == 5) {
                handRanks = _oppHand5.cards.ConvertAll(cardN => cardN.GetComponent<CardView>().controller.model.rank);
                if (handRanks.Contains(cardRank) && _oppHand5.cards.Count == rowThreshold) {
                    handNumber = 5;
                    break;
                }
            }
        }

        // TODO: Search for flushes and straights. 
        if (handNumber == 0) {
            if (_oppHand1.cards.Count == rowThreshold) {
                handNumber = 1;
            } else if (_oppHand2.cards.Count == rowThreshold) {
                handNumber = 2;
            } else if (_oppHand3.cards.Count == rowThreshold) {
                handNumber = 3;
            } else if (_oppHand4.cards.Count == rowThreshold) {
                handNumber = 4;
            } else {
                handNumber = 5;
            }
        }

        moveOppPlayableCardToHand(handNumber);
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

    IEnumerator performEndgame() {
        yield return new WaitForSeconds(1.0f);
        animateFlipLastCard();
    }

    void analyzeGame() {

    }

    void animateFlipLastCard() {
        GameObject card;
        for (int i = 1; i <= 5; i++) {
            if (i == 1) {
                card = _oppHand1.cards[4];
            } else if (i == 2) {
                card = _oppHand2.cards[4];
            } else if (i == 3) {
                card = _oppHand3.cards[4];
            } else if (i == 4) {
                card = _oppHand4.cards[4];
            } else {
                card = _oppHand5.cards[4];
            }
            iTween.RotateTo(
                card.gameObject,
                iTween.Hash(
                    "rotation", new Vector3(0, 0, -360),
                    "time", 0.1f,
                    "delay", 0.1f * i,
                    "easetype", iTween.EaseType.linear
                )
            );
        }
        _isGameOn = true;
    }
    
    void resetGame() {
        _canDraw = true;
        _playerTotalCards = 0;
        _oppTotalCards = 0;
        _deck.cards.AddRange(_hand1.cards);
        _hand1.cards.Clear();
        _deck.cards.AddRange(_hand2.cards);
        _hand2.cards.Clear();
        _deck.cards.AddRange(_hand3.cards);
        _hand3.cards.Clear();
        _deck.cards.AddRange(_hand4.cards);
        _hand4.cards.Clear();
        _deck.cards.AddRange(_hand5.cards);
        _hand5.cards.Clear();
        _deck.cards.AddRange(_oppHand1.cards);
        _oppHand1.cards.Clear();
        _deck.cards.AddRange(_oppHand2.cards);
        _oppHand2.cards.Clear();
        _deck.cards.AddRange(_oppHand3.cards);
        _oppHand3.cards.Clear();
        _deck.cards.AddRange(_oppHand4.cards);
        _oppHand4.cards.Clear();
        _deck.cards.AddRange(_oppHand5.cards);
        _oppHand5.cards.Clear();
        _playableHand.cards.Clear();
        _oppPlayableHand.cards.Clear();

        for (int i = 0; i < _deck.cards.Count; i++) {
            _deck.cards[i].transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
        shuffleDeck();
    }

}
