using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnableModule : MonoBehaviour
{
    public float runXPos => transform.position.x + runXLocalPos;

    [ReadOnly] public RunnableModule next;
    [ReadOnly] public float runXLocalPos;
    [ReadOnly] public float moveToNextTime;

    [ReadOnly] public bool jumpAttempted = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPlayerLeft()
    {
        if (!jumpAttempted)
        {
            Fail();
            PlayerModel.Instance.JumpMissed();
        }
    }

    public virtual void Ready()
    {

    }

    public virtual void Perfect()
    {

    }

    public virtual void Acceptable()
    {

    }

    public virtual void Fail()
    {

    }
}