using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnsetEditor : MonoBehaviour
{
    public AudioSource audioSource;
    public TextAsset onsetDataAsset;
    public GameObject onsetIndicatorPrefab;
    public Camera camera;

    public NormalizedPositioner seeker;
    public Transform onsetContainer;

    // Start is called before the first frame update
    void Start()
    {
        ResetData();
    }

    // Update is called once per frame
    void Update()
    {
        seeker.position = HelperUtilities.Remap(audioSource.time, 0, audioSource.clip.length, 0, 1);
    }

    void ResetData()
    {
        onsetContainer.DestroyAllChildren();

        var onsets = GameTools.GetOnsets(onsetDataAsset.text);
        foreach (var onset in onsets)
        {
            var onsetIndicator = Instantiate(onsetIndicatorPrefab, onsetContainer).GetComponent<OnsetIndicator>();
            onsetIndicator.position = HelperUtilities.Remap(onset.time, 0, audioSource.clip.length, 0, 1);
            onsetIndicator.amplitude = onset.amplitude;
        }
    }
}
