using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSCore;
using CSCore.Codecs;
using OnsetDetection;
using UnityEngine;
using UnityEngine.Networking;

public class OnsetDetectionVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform beatsContainer;
    public GameObject beatIndicatorPrefab;

    public float moveSpeed = 1f;

    public TextAsset beatsData;

    // Start is called before the first frame update
    void Start()
    {
        SpawnBeatIndicators();
    }

    // Update is called once per frame
    void Update()
    {
        MoveBeats();
    }

    void SpawnBeatIndicators()
    {
        var onsets = GetOnsets();

        for (int i = 0; i < onsets.Count; i++)
        {
            var beatIndicator = Instantiate(beatIndicatorPrefab, beatsContainer);
            beatIndicator.transform.localPosition = new Vector3(onsets[i] * moveSpeed, 0, 0);
        }
    }

    void MoveBeats()
    {
        if (audioSource && audioSource.clip)
        {
            beatsContainer.localPosition = new Vector3(-audioSource.time * moveSpeed, 0, 0);
        }
    }

    List<float> GetOnsets()
    {
        var lines = beatsData.text.Split('\n');
        return lines.Where(s => s.Contains(',')).Select(s => float.Parse(s.Split(',')[0])).ToList();
    }
}