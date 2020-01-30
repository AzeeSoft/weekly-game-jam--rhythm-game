using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI title;

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
