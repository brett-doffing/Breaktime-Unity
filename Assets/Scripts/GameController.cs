using System;
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
    public GameCanvas gameCanvas;

    public GameObject spot1;
    public GameObject spot2;
    public GameObject spot3;
    public GameObject spot4;
    public GameObject spot5;
    public GameObject oppSpot1;
    public GameObject oppSpot2;
    public GameObject oppSpot3;
    public GameObject oppSpot4;
    public GameObject oppSpot5;
    
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
    PokerAnalyzer analyzer = new PokerAnalyzer();
    List<HandModel> _playerEndGameHands;
    List<HandModel> _oppEndGameHands;
    bool _canDraw = true;
    bool _isGameOn = true;
    int _playerTotalCards = 0;
    int _oppTotalCards = 0;
    int _playerPoints = 0;
    int _oppPoints = 0;

    Material _mat;
    Color _originalHolderColor;

    // Start is called before the first frame update
    void Start()
    {
        if (Camera.main.aspect < 0.65f) {
            Camera.main.orthographicSize = 60f * (0.46f / Camera.main.aspect);
        } else {
            Camera.main.orthographicSize = 45f * (0.75f / Camera.main.aspect);
        }
        
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

        _mat = Resources.Load("Materials/Holders") as Material;
        _originalHolderColor = _mat.color;

        spot1.GetComponent<Renderer>().material = Instantiate(_mat);
        spot2.GetComponent<Renderer>().material = Instantiate(_mat);
        spot3.GetComponent<Renderer>().material = Instantiate(_mat);
        spot4.GetComponent<Renderer>().material = Instantiate(_mat);
        spot5.GetComponent<Renderer>().material = Instantiate(_mat);
        oppSpot1.GetComponent<Renderer>().material = Instantiate(_mat);
        oppSpot2.GetComponent<Renderer>().material = Instantiate(_mat);
        oppSpot3.GetComponent<Renderer>().material = Instantiate(_mat);
        oppSpot4.GetComponent<Renderer>().material = Instantiate(_mat);
        oppSpot5.GetComponent<Renderer>().material = Instantiate(_mat);

        // spot1.GetComponent<Renderer>().material.color = Color.cyan;

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
                
                newCard.gameObject.GetComponent<CustomCard>().setupWith(cardController);
                _deck.cards.Add(newCard.gameObject);

                // string s = Convert.ToString(model.id, 2);
                // Debug.LogFormat("{0}_{1}\n  {2}: {3}", model.rank, model.suit, model.id, s);
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
        switch (id)
        {
            case 0:
                if (_deck.cards.Count == 52) {
                    dealCards();
                } else if (_deck.cards.Count > 2 && _canDraw == true) {
                    addPlayableCard();
                } else if (_deck.cards.Count == 2 && _isGameOn) {
                    resetGame();
                }
                break;
            case > 0 and <= 5:
                if (_playableHand.cards.Count == 1) {
                    movePlayableCardToHand(id);
                }
                break;
            default:
                break;
        }
    }

    void dealCards() {
        if (_deck.cards.Count == 52) {
            for (int i = 1; i <= 10; i++) {
                GameObject card = _deck.cards[_deck.cards.Count - 1];
                CustomCard cardComponent = card.GetComponent<CustomCard>();
                animateDealtCard(card, i);
                _deck.cards.Remove(card);
                cardComponent.controller.model.isFaceUp = !cardComponent.controller.model.isFaceUp;
            }
            _playerTotalCards += 5;
            _oppTotalCards += 5;
        }
        // Debug.LogFormat("Hand 1 count = {0}", _hand1.cards.Count);
    }

    void addPlayableCard() {
        // Do not initially increment cards in hand
        if (_playableHand.cards.Count == 1) {
            _playerTotalCards += 1;
            _playableHand.cards.Clear();
            searchOppRow();
        }
        GameObject card = _deck.cards[_deck.cards.Count - 1];
        CustomCard cardComponent = card.GetComponent<CustomCard>();
        _deck.cards.Remove(card);
        _playableHand.cards.Add(card);
        _canDraw = false;


        animateDrawPlayableCard(card);
        
        cardComponent.controller.model.isFaceUp = !cardComponent.controller.model.isFaceUp;
    }

    void addOppPlayableCard() {
        GameObject card = _deck.cards[_deck.cards.Count - 1];
        CustomCard cardComponent = card.GetComponent<CustomCard>();
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
                "time", 0.25f,
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
                "time", 0.25f,
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
                "time", 0.25f,
                "easetype", iTween.EaseType.linear
            )
        );
        iTween.MoveTo(
            card.gameObject,
            iTween.Hash(
                "position", new Vector3(playableHand.transform.position.x, 0.1f, 0),
                "time", 0.25f,
                "easetype", iTween.EaseType.linear
            )
        );
    }

    void animateDrawOppPlayableCard(GameObject card) {
        iTween.MoveTo(
            card.gameObject,
            iTween.Hash(
                "position", new Vector3(oppPlayableHand.transform.position.x, 0.1f, 0),
                "time", 0.25f,
                "easetype", iTween.EaseType.linear
            )
        );
        if (_oppTotalCards < 20) {
            iTween.RotateTo(
                card.gameObject,
                iTween.Hash(
                    "rotation", new Vector3(0, 0, -360),
                    "time", 0.25f,
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
                }
                break;
            case 2:
                if (_hand2.cards.Count == row) {
                    _hand2.addToHand(card);
                }
                break;
            case 3:
                if (_hand3.cards.Count == row) {
                    _hand3.addToHand(card);
                }
                break;
            case 4:
                if (_hand4.cards.Count == row) {
                    _hand4.addToHand(card);
                }
                break;
            case 5:
                if (_hand5.cards.Count == row) {
                    _hand5.addToHand(card);
                }
                break;
            default:
                break;
        }
        _canDraw = true;

        if (_oppPlayableHand.cards.Count == 0) { addOppPlayableCard(); }
    }

    void moveOppPlayableCardToHand(int handNumber) {
        GameObject card = _oppPlayableHand.cards[0];
        // CustomCard cardComponent = card.GetComponent<CustomCard>();
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
        CustomCard cardComponent = card.GetComponent<CustomCard>();
        int cardRank = cardComponent.controller.model.rank;
        int handNumber = 0;

        List<int> handRanks;
        for (int i = 1; i <= 5; i++) {
            if (i == 1) {
                handRanks = _oppHand1.cards.ConvertAll(cardN => cardN.GetComponent<CustomCard>().controller.model.rank);
                if (handRanks.Contains(cardRank) && _oppHand1.cards.Count == rowThreshold) {
                    handNumber = 1;
                    break;
                }
            } else if (i == 2) {
                handRanks = _oppHand2.cards.ConvertAll(cardN => cardN.GetComponent<CustomCard>().controller.model.rank);
                if (handRanks.Contains(cardRank) && _oppHand2.cards.Count == rowThreshold) {
                    handNumber = 2;
                    break;
                }
            } else if (i == 3) {
                handRanks = _oppHand3.cards.ConvertAll(cardN => cardN.GetComponent<CustomCard>().controller.model.rank);
                if (handRanks.Contains(cardRank) && _oppHand3.cards.Count == rowThreshold) {
                    handNumber = 3;
                    break;
                }
            } else if (i == 4) {
                handRanks = _oppHand4.cards.ConvertAll(cardN => cardN.GetComponent<CustomCard>().controller.model.rank);
                if (handRanks.Contains(cardRank) && _oppHand4.cards.Count == rowThreshold) {
                    handNumber = 4;
                    break;
                }
            } else if (i == 5) {
                handRanks = _oppHand5.cards.ConvertAll(cardN => cardN.GetComponent<CustomCard>().controller.model.rank);
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
        analyzeGame();
        gameCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        for (int i = 1; i <= 5; i++) {
            GameObject card;
            GameObject spot;
            GameObject oppSpot;
            if (i == 1) {
                card = _oppHand1.cards[4];
                spot = spot1;
                oppSpot = oppSpot1;
            } else if (i == 2) {
                card = _oppHand2.cards[4];
                spot = spot2;
                oppSpot = oppSpot2;
            } else if (i == 3) {
                card = _oppHand3.cards[4];
                spot = spot3;
                oppSpot = oppSpot3;
            } else if (i == 4) {
                card = _oppHand4.cards[4];
                spot = spot4;
                oppSpot = oppSpot4;
            } else {
                card = _oppHand5.cards[4];
                spot = spot5;
                oppSpot = oppSpot5;
            }
                
            yield return new WaitForSeconds(1.0f);
            spot.GetComponent<Renderer>().material.color = Color.yellow;
            string playerText = _playerEndGameHands[i - 1].getHandRankString();            
            gameCanvas.showPlayerText(playerText);
            yield return new WaitForSeconds(1.0f);
            animateFlipLastCard(card);
            yield return new WaitForSeconds(1.0f);
            oppSpot.GetComponent<Renderer>().material.color = Color.yellow;
            string oppText = _playerEndGameHands[i - 1].getHandRankString();
            gameCanvas.showOppText(oppText);            
            yield return new WaitForSeconds(1.0f);
            spot.GetComponent<Renderer>().material.color = _originalHolderColor;
            oppSpot.GetComponent<Renderer>().material.color = _originalHolderColor;
            getLostHand(i - 1);
            gameCanvas.hideHandLabels();            
            yield return new WaitForSeconds(1.0f);
        }
        showFinalCanvas();
        yield return new WaitForSeconds(3.0f);
        gameCanvas.hideHandLabels();
        gameCanvas.gameObject.SetActive(false);
        _isGameOn = true;
    }

    void showCanvasForHand(int index) {
        if (_playerEndGameHands[index].handWon) {
            _playerPoints += 1;
        } else if (_oppEndGameHands[index].handWon) {
            _oppPoints += 1;
        }
        string playerText = _playerEndGameHands[index].getHandRankString();
        string oppText = _oppEndGameHands[index].getHandRankString();
        gameCanvas.showCanvasWith(playerText, oppText);
    }

    void showFinalCanvas() {
        // string oppText;
        // if (_playerPoints > _oppPoints) {
        //     oppText = "You Won!";
        // } else if (_playerPoints < _oppPoints) {
        //     oppText = "You Lost";
        // } else {
        //     oppText = "Tie Game";
        // }
        // string playerText = String.Format("{0} - {1}", _playerPoints, _oppPoints);

        for (int i = 0; i < 5; i++) {
            if (_playerEndGameHands[i].handWon) {
                _playerPoints += 1;
            } else if (_oppEndGameHands[i].handWon) {
                _oppPoints += 1;
            }
        }
        gameCanvas.showPlayerText(_playerPoints.ToString());
        gameCanvas.showOppText(_oppPoints.ToString());
        // gameCanvas.showCanvasWith(playerText, oppText, true);
    }

    void animateFlipLastCard(GameObject card) {
        iTween.RotateTo(
            card.gameObject,
            iTween.Hash(
                "rotation", new Vector3(0, 0, -360),
                "time", 0.25f,
                "delay", 0.1f,
                "easetype", iTween.EaseType.linear
            )
        );
    }

    void getLostHand(int index) {
        if (_playerEndGameHands[index].handWon) {
            if (index == 0)
                animateLostHand(_oppHand1);
            else if (index == 1)
                animateLostHand(_oppHand2);
            else if (index == 2)
                animateLostHand(_oppHand3);
            else if (index == 3)
                animateLostHand(_oppHand4);
            else 
                animateLostHand(_oppHand5);
        } else {
            if (index == 0)
                animateLostHand(_hand1);
            else if (index == 1)
                animateLostHand(_hand2);
            else if (index == 2)
                animateLostHand(_hand3);
            else if (index == 3)
                animateLostHand(_hand4);
            else 
                animateLostHand(_hand5);
        }
    }

    void animateLostHand(CardContainer hand) {
        for (int i = 0; i< 5; i++) {
            iTween.MoveBy(
            hand.cards[i].gameObject,
            iTween.Hash(
                "z", i * 2,
                "time", 0.25f,
                "easetype", iTween.EaseType.linear
            )
        );
        }
    }

    void analyzeGame() {
        HandModel aHand1 = _hand1.getHandModel();
        HandModel aHand2 = _hand2.getHandModel();
        HandModel aHand3 = _hand3.getHandModel();
        HandModel aHand4 = _hand4.getHandModel();
        HandModel aHand5 = _hand5.getHandModel();
        HandModel aOppHand1 = _oppHand1.getHandModel();
        HandModel aOppHand2 = _oppHand2.getHandModel();
        HandModel aOppHand3 = _oppHand3.getHandModel();
        HandModel aOppHand4 = _oppHand4.getHandModel();
        HandModel aOppHand5 = _oppHand5.getHandModel();

        List<HandModel> hands = new List<HandModel>() {aHand1, aHand2, aHand3, aHand4, aHand5};
        List<HandModel> oppHands = new List<HandModel>() {aOppHand1, aOppHand2, aOppHand3, aOppHand4, aOppHand5};
        analyzer.analyzeHands(hands, oppHands, analyzerCallback);
    }

    void analyzerCallback(List<HandModel> playerHands, List<HandModel> oppHands) {
        _playerEndGameHands = playerHands;
        _oppEndGameHands = oppHands;
    }
    
    void resetGame() {
        _canDraw = true;
        _playerTotalCards = 0;
        _oppTotalCards = 0;
        _playerPoints = 0;
        _oppPoints = 0;
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
        gameCanvas.gameObject.SetActive(false);

        for (int i = 0; i < _deck.cards.Count; i++) {
            _deck.cards[i].transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
        shuffleDeck();
    }

}
