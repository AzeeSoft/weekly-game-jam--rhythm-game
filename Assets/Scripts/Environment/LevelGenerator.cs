﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BasicTools.ButtonInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[ExecuteInEditMode]
public class LevelGenerator : SingletonMonoBehaviour<LevelGenerator>
{
    public float yMultiplier => playerModel.playerSpeed;

    [Header("Local References")] public Camera camera;
    public LevelManager levelManager;
    public PlayerModel playerModel;
    public Transform leftContainer;
    public Transform rightContainer;

    [Header("Asset References")] public TextAsset beatsData;
    public GameObject wallModulePrefab;

    [SerializeField] [Button("Generate Level", "GenerateLevel")]
    private bool btn_GenerateLevel;

    [SerializeField] [Button("Clear Level", "ClearLevel")]
    private bool btn_ClearLevel;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AlignContainers();
    }

    List<float> GetBeats()
    {
        var lines = beatsData.text.Split('\n');
        return lines.Where(s => s.Contains(',')).Select(s => float.Parse(s.Split(',')[0])).ToList();
    }

    void AlignContainers()
    {
        var camBounds = camera.GetCameraBounds();

        if (leftContainer)
        {
            var newPos = leftContainer.position;
            newPos.x = camBounds.topLeft.x;
            leftContainer.position = newPos;
        }

        if (rightContainer)
        {
            var newPos = rightContainer.position;
            newPos.x = camBounds.bottomRight.x;
            rightContainer.position = newPos;
        }
    }

    public void GenerateLevel()
    {
        ClearLevel();

        var beats = GetBeats();

        Transform curContainer = rightContainer;
        RunnableModule lastRunnableModule = null;

        for (int i = 0; i < beats.Count; i++)
        {
            float spawnYPos = 0;
            float beatYPos = beats[i] * yMultiplier;

            if (i > 0)
            {
                spawnYPos = beats[i - 1] * yMultiplier;
            }

            WallModule wallModule;
            if (Application.isPlaying)
            {
                wallModule = Instantiate(wallModulePrefab, curContainer).GetComponent<WallModule>();
            }
            else
            {
#if UNITY_EDITOR
                wallModule = (PrefabUtility.InstantiatePrefab(wallModulePrefab, curContainer) as GameObject).GetComponent<WallModule>();
#endif
            }

            var newPos = new Vector3(0, spawnYPos, 0);

            var wallBounds = wallModule.GetComponent<Collider2D>().bounds;
            if (curContainer == leftContainer)
            {
                newPos.x = wallBounds.extents.x;
                wallModule.runXLocalPos = wallBounds.extents.x;
            }
            else
            {
                newPos.x = -wallBounds.extents.x;
                wallModule.runXLocalPos = -wallBounds.extents.x;
            }

            wallModule.transform.localPosition = newPos;

            var newScale = wallModule.transform.localScale;
            newScale.y = beatYPos - spawnYPos;
            wallModule.transform.localScale = newScale;

            wallModule.moveToNextTime = beats[i];

            curContainer = curContainer == leftContainer ? rightContainer : leftContainer;

            if (lastRunnableModule)
            {
                lastRunnableModule.next = wallModule;
            }
            lastRunnableModule = wallModule;
        }
    }

    public void ClearLevel()
    {
        leftContainer.DestroyAllChildren(true);
        rightContainer.DestroyAllChildren(true);
    }
}