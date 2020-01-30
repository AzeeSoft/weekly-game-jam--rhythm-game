using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public AudioSource levelMusicSource;
    public string mainMenuSceneName = "MainMenu";
    public CinemachineBrain brain;

    public List<RunnableModule> allRunnableModules { get; private set; } = new List<RunnableModule>();

    new void Awake()
    {
        base.Awake();

        Time.timeScale = 1;

        RefreshAllRunnableModules();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void RefreshAllRunnableModules()
    {
        allRunnableModules.Clear();

        allRunnableModules = LevelGenerator.Instance.GetComponentsInChildren<RunnableModule>().ToList();
        allRunnableModules.Sort((m1, m2) => m1.moveToNextTime.CompareTo(m2.moveToNextTime));
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        var sceneTransitionHelper = SceneTransitionHelper.Create();
        sceneTransitionHelper.data["dontHideDivider"] = true;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}