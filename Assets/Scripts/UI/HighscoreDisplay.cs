using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour
{
    public HighscoreManagerScript hsm;
    public TMPro.TMP_InputField inputField;
    public Button nameButton;
    public List<TMPro.TextMeshProUGUI> textMeshes;

    public float accuracy;
    public string playerName = "";

    private bool shouldUpdate = false;

    void Start()
    {
        displayData();
        checkScore();
    }

    public void checkScore()
    {
        if (hsm.IsHighScore(accuracy, "Sweden"))
        {
            Debug.Log("IsHighScore is true");
            inputField.gameObject.SetActive(true);
            nameButton.gameObject.SetActive(true);
            shouldUpdate = true;
        }
    }

    public void getName()
    {
        playerName = inputField.text;
    }

    public void displayData()
    {
        if (shouldUpdate)
        {
            hsm.AddNewHighScore(accuracy, playerName, "Sweden");
        }

        List<HighScoreData> scoreData = hsm.GetHighScores("Sweden");

        for (int i = 0; i < scoreData.Count; ++i)
        {
           textMeshes[i].text = scoreData[i].name.ToString() + "              " + scoreData[i].accuracy.ToString();
        }
    }

}
