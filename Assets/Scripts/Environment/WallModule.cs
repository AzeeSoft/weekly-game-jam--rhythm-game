using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallModule : RunnableModule
{
    public Renderer wallRenderer;

    // Start is called before the first frame update
    void Start()
    {
        wallRenderer.material.SetVector("_localScale", transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
