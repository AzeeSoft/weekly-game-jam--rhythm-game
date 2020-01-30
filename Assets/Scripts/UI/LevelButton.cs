using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI bestAccuracyText;
    public TextMeshProUGUI durationText;

    private LevelData levelData;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(LevelData levelData)
    {
        title.text = levelData.levelName;

        float bestAccuracy = HighscoreManagerScript.Instance.GetBestScore(levelData.levelKey);

        if (bestAccuracy > 0)
        {
            var bestAccuracyRemapped =
                HelperUtilities.Remap(bestAccuracy, 0, PlayerModel.maxScore, 0, 100f);
            bestAccuracyText.text = $"{bestAccuracyRemapped:##0.##}%";
        }
        else
        {
            bestAccuracyText.text = "";
        }

        float duration = levelData.audioClip.length;
        int mins = (int)(duration / 60);
        int secs = (int)(duration % 60);

        durationText.text = $"{mins:#0}:{secs:00}";

        this.levelData = levelData;
    }

    public void GoToLevel()
    {
        if (levelData)
        {
            MainMenu.Instance.GoToScene(levelData.sceneName);
        }
    }
    
}
