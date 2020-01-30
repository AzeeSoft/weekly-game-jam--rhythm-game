using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WallShaderHelper : MonoBehaviour
{
    public Renderer wallRenderer;

    public bool currentlyEditing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var material = Application.isPlaying ? wallRenderer.material : wallRenderer.sharedMaterial;

        if (Application.isPlaying || currentlyEditing)
        {
            material.SetVector("_localScale", transform.localScale);
            material.SetVector("_topPosition", wallRenderer.transform.TransformPoint(new Vector3(0, 0.5f, 0)));
        }
    }

    public void HidePerfectTimeIndicator()
    {
        wallRenderer.material.SetFloat("_perfectTimeIndicatorHeight", 0);
    }
}
