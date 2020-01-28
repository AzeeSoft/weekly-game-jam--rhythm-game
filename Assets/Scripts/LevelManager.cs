using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public AudioSource levelMusicSource;

    public List<RunnableModule> allRunnableModules { get; private set; } = new List<RunnableModule>();

    new void Awake()
    {
        base.Awake();

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
}