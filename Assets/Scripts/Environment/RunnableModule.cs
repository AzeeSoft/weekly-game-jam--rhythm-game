using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnableModule : MonoBehaviour
{
    public float runXPos => transform.position.x + runXLocalPos;

    [ReadOnly] public RunnableModule next;
    [ReadOnly] public float runXLocalPos;
    [ReadOnly] public float moveToNextTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}