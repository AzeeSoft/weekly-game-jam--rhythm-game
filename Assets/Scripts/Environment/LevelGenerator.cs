using System.Collections;
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

    [Header("Asset References")] public TextAsset onsetData;
    public GameObject wallModulePrefab;

    [Header("Config")] public float minOnsetInterval;
    public float startRunDuration = 3f;
    public float endScreenDelay = 3f;
    public float endRunDuration = 6f;

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

    void AlignContainers()
    {
        if (camera)
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
    }

    public void GenerateLevel()
    {
        ClearLevel();

        var beats = GameTools.GetOnsets(onsetData.text).Select((onsetInfo) => onsetInfo.time).ToList();

        Transform curContainer = leftContainer;
        RunnableModule lastRunnableModule = null;

        var startingWallModule = SpawnWall(curContainer, -startRunDuration * yMultiplier, 0);
        startingWallModule.autoRun = true;
        startingWallModule.playMusicOnLeave = true;
        lastRunnableModule = startingWallModule;

        curContainer = curContainer == leftContainer ? rightContainer : leftContainer;
        int lastBeatSpawned = -1;
        for (int i = 0; i < beats.Count; i++)
        {
            float spawnYPos = 0;
            float beatYPos = beats[i] * yMultiplier;

            if (lastBeatSpawned >= 0)
            {
                spawnYPos = beats[lastBeatSpawned] * yMultiplier;
            }

            if (beatYPos - spawnYPos < (minOnsetInterval * yMultiplier))
            {
                continue;
            }

            var wallModule = SpawnWall(curContainer, spawnYPos, beatYPos);

            curContainer = curContainer == leftContainer ? rightContainer : leftContainer;

            if (lastRunnableModule)
            {
                lastRunnableModule.next = wallModule;
            }
            lastRunnableModule = wallModule;
            lastBeatSpawned = i;
        }


        float endSpawnYPos = 0;
        if (lastBeatSpawned >= 0)
        {
            endSpawnYPos = beats[lastBeatSpawned] * yMultiplier;
        }

        var endingWallModule = SpawnWall(curContainer, endSpawnYPos, endSpawnYPos + endRunDuration * yMultiplier);
        endingWallModule.autoRun = true;
        endingWallModule.endLevelOnLeave = true;
        if (lastRunnableModule)
        {
            lastRunnableModule.next = endingWallModule;
        }
    }

    WallModule SpawnWall(Transform curContainer, float spawnYPos, float beatYPos)
    {
        WallModule wallModule = null;
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
        newScale.x = Mathf.Abs(newScale.x) * (curContainer == leftContainer ? 1 : -1);
        newScale.y = (beatYPos - spawnYPos) + (playerModel.maxAcceptableTimeThreshold * yMultiplier);
        wallModule.transform.localScale = newScale;

        wallModule.moveToNextTime = beatYPos / yMultiplier;

        return wallModule;
    }
    public void ClearLevel()
    {
        leftContainer.DestroyAllChildren(true);
        rightContainer.DestroyAllChildren(true);
    }
}