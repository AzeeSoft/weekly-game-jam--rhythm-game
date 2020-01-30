using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HUD : SingletonMonoBehaviour<HUD>
{
    public bool isPaused => Time.timeScale <= 0;


    public TextMeshProUGUI timeLeftText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI endAccuracyText;
    public TextMeshProUGUI endBestAccuracyText;
    public GameObject pauseScreen;
    public GameObject endScreen;

    public float screenTransitionDuration = 0.3f;
    public float closeDoorsDuration = 1f;

    private bool wasMusicPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAccuracy();
        UpdateTimeLeft();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void UpdateAccuracy()
    {
        var accuracyRemapped = HelperUtilities.Remap(PlayerModel.Instance.accuracy, 0, PlayerModel.maxScore, 0, 100f);
        accuracyText.text = $"Accuracy: {accuracyRemapped:##0.##}%";
    }

    void UpdateTimeLeft()
    {
        float timeLeft = LevelManager.Instance.levelMusicSource.clip.length -
                         LevelManager.Instance.levelMusicSource.time;
        int mins = (int) (timeLeft / 60);
        int secs = (int) (timeLeft % 60);

        timeLeftText.text = $"{mins:#0}:{secs:00}";
    }

    public void PlayAgain()
    {
        DoorUI.Instance.CloseDoors(closeDoorsDuration, () => { LevelManager.Instance.RestartCurrentScene(); });
    }

    public void GoToMainMenu()
    {
        DoorUI.Instance.CloseDoors(closeDoorsDuration, () => { LevelManager.Instance.GoToMainMenu(); });
    }

    public void Pause()
    {
        ShowScreen(pauseScreen);
        Time.timeScale = 0;

        wasMusicPlaying = LevelManager.Instance.levelMusicSource.isPlaying;
        if (wasMusicPlaying)
        {
            LevelManager.Instance.levelMusicSource.Pause();
        }
    }

    public void Resume()
    {
        HideScreen(pauseScreen, () =>
        {
            Time.timeScale = 1;
            if (wasMusicPlaying)
            {
                wasMusicPlaying = false;
                LevelManager.Instance.levelMusicSource.UnPause();
            }
        });
    }

    public void ShowEndScreen(float delay)
    {
        this.WaitAndExecute(() =>
        {
            Time.timeScale = 0;

            var accuracyRemapped =
                HelperUtilities.Remap(PlayerModel.Instance.accuracy, 0, PlayerModel.maxScore, 0, 100f);
            endAccuracyText.text = $"{accuracyRemapped:##0.##}%";

            float bestAccuracy = PlayerModel.Instance.accuracy;
            if (LevelManager.Instance.levelData)
            {
                HighscoreManagerScript.Instance.UpdateBestScore(LevelManager.Instance.levelData.levelKey,
                    PlayerModel.Instance.accuracy);
                bestAccuracy = HighscoreManagerScript.Instance.GetBestScore(LevelManager.Instance.levelData.levelKey);
            }

            var bestAccuracyRemapped =
                HelperUtilities.Remap(bestAccuracy, 0, PlayerModel.maxScore, 0, 100f);
            endBestAccuracyText.text = $"{bestAccuracyRemapped:##0.##}%";

            ShowScreen(endScreen);
        }, delay);
    }

    void ShowScreen(GameObject screen)
    {
        var preSeq = screen.DOFade(0, 0);
        var fadeInSeq = screen.DOFade(1f, screenTransitionDuration);

        preSeq.AppendCallback(() =>
        {
            screen.SetActive(true);
            fadeInSeq.Play();
        });
        preSeq.Play();
    }

    void HideScreen(GameObject screen, Action callback = null)
    {
        screen.SetActive(true);

        var preSeq = screen.DOFade(1, 0);
        var fadeOutSeq = screen.DOFade(0f, screenTransitionDuration);

        fadeOutSeq.AppendCallback(() =>
        {
            screen.SetActive(false);
            callback?.Invoke();
        });

        preSeq.AppendCallback(() => { fadeOutSeq.Play(); });
        preSeq.Play();
    }
}