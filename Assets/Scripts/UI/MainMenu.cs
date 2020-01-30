using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class MainMenu : SingletonMonoBehaviour<MainMenu>
{
    public MainMenuPage mainPage;
    public GameObject mainMenuTimeline;

    public Transform levelsContainer;
    public GameObject levelButtonPrefab;
    public List<LevelData> levels;

    new void Awake()
    {
        base.Awake();

        Time.timeScale = 1;
        mainPage.gameObject.SetActive(false);

        RefreshLevels();

        if (!SceneTransitionHelper.Instance || !SceneTransitionHelper.Instance.data.ContainsKey("dontHideDivider"))
        {
            DoorUI.Instance.HideDividers(0);
        }

        SceneTransitionHelper.Instance?.Destroy();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void RefreshLevels()
    {
        levelsContainer.DestroyAllChildren();

        foreach (var levelData in levels)
        {
            var levelButton = Instantiate(levelButtonPrefab, levelsContainer).GetComponent<LevelButton>();
            levelButton.Init(levelData);
        }
    }

    public void ShowGraphicObject(GameObject obj, float duration)
    {
        obj.DOFade(0, 0).AppendCallback(() =>
        {
            obj.SetActive(true);
            obj.DOFade(1, duration).Play();
        }).Play();
    }

    public void GoToScene(string sceneName)
    {
        mainMenuTimeline.SetActive(false);
        DoorUI.Instance.CloseDoors(-1, () => { SceneManager.LoadScene(sceneName); });
    }

    public void Exit()
    {
        DoorUI.Instance.CloseDoors(-1, () =>
        {
            ScreenFader.Instance.FadeOut(-1, () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif

                Application.Quit();
            });
        });
    }
}