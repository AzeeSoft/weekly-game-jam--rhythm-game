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

    [ReadOnly] public bool autoRun = false;
    [ReadOnly] public bool playMusicOnLeave = false;
    [ReadOnly] public bool endLevelOnLeave = false;

    private bool playerLeft = false;

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
        if (!playerLeft)
        {
            if (autoRun)
            {
                Perfect();
            }

            if (!autoRun && !jumpAttempted)
            {
                Fail();
                PlayerModel.Instance.JumpMissed();
            }

            if (playMusicOnLeave)
            {
                LevelManager.Instance.levelMusicSource.Play();
            }

            if (endLevelOnLeave)
            {
                // TODO: End Level Sequence
            }

            playerLeft = true;
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