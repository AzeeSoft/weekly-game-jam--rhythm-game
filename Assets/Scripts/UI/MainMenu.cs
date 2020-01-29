using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public MainMenuPage mainPage;

    void Awake()
    {
        Time.timeScale = 1;
        mainPage.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        ScreenFader.Instance.FadeOut(-1, () => { SceneManager.LoadScene(sceneName); });
    }

    public void Exit()
    {
        ScreenFader.Instance.FadeOut(-1, () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        });
    }
}
