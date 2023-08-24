using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameCanvas : MonoBehaviour
{
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI oppText;

    void Start() {
        playerText.enabled = false;
        oppText.enabled = false;
    }

    public void showCanvasWith(string playerLabel, string oppLabel, bool showButton = false) {
        playerText.text = playerLabel;
        oppText.text = oppLabel;
        gameObject.SetActive(true);
    }

    public void hideHandLabels() {
        playerText.enabled = false;
        oppText.enabled = false;
    }

    public void showPlayerText(string text) {
        playerText.enabled = true;
        playerText.text = text;
    }

    public void showOppText(string text) {
        oppText.enabled = true;
        oppText.text = text;
    }
}
