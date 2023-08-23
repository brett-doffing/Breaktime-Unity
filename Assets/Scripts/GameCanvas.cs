using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameCanvas : MonoBehaviour
{
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI oppText;

    public void showCanvasWith(string playerLabel, string oppLabel, bool showButton = false) {
        playerText.text = playerLabel;
        oppText.text = oppLabel;
        gameObject.SetActive(true);
    }
}
