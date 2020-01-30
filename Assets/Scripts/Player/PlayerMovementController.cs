using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private AudioSource levelMusicSource => LevelManager.Instance.levelMusicSource;
    private float playerSpeed => playerModel.playerSpeed;

    public float lerpSpeed = 1f;
    public float reachDistance = 0.2f;
    public Transform feetRef;

    private PlayerModel playerModel;
    public RunnableModule curRunnableModule { get; private set; }

    void Awake()
    {
        playerModel = GetComponent<PlayerModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        curRunnableModule = LevelManager.Instance.allRunnableModules[0];
        curRunnableModule.Ready();

        var adjustedPos = transform.position;
        adjustedPos.y = curRunnableModule.transform.position.y;
        transform.position = adjustedPos;

        playerModel.trailRenderer.Clear();

        print(curRunnableModule);
        print(curRunnableModule.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckAndUpdateRunnableModule();
        CheckForPlayerJump();
        Move();
    }

    void Move()
    {
        var adjustedXPos = curRunnableModule.runXPos + (transform.position.x > curRunnableModule.runXPos ? 1 : -1) *
                           Mathf.Abs(transform.position.x - feetRef.position.x);

        Vector3 targetPos;
        if (curRunnableModule.autoRun)
        {
            targetPos = new Vector3(Mathf.Lerp(transform.position.x, adjustedXPos, Time.deltaTime * lerpSpeed),
                transform.position.y + Time.deltaTime * playerSpeed);
        }
        else
        {
            targetPos = new Vector3(Mathf.Lerp(transform.position.x, adjustedXPos, Time.deltaTime * lerpSpeed),
                levelMusicSource.time * playerSpeed);
        }

        transform.position = targetPos;

        //print(adjustedXPos - transform.position.x);

        bool isSwitching = Mathf.Abs(adjustedXPos - transform.position.x) > reachDistance;

        if (!isSwitching)
        {
            var newScale = transform.localScale;
            newScale.x = Mathf.Abs(newScale.x) * (curRunnableModule.runXLocalPos < 0 ? -1 : 1);
            transform.localScale = newScale;
        }
    }

    public void JumpToNextRunnableModule()
    {
        curRunnableModule.OnPlayerLeft();
        if (curRunnableModule.next)
        {
            curRunnableModule = curRunnableModule.next;
            curRunnableModule.Ready();
        }
    }

    void CheckAndUpdateRunnableModule()
    {
        bool check = curRunnableModule.next || curRunnableModule.autoRun;
        while (check)
        {
            check = false;
            if (curRunnableModule.autoRun && (transform.position.y >= curRunnableModule.moveToNextTime * playerSpeed))
            {
                JumpToNextRunnableModule();
                check = true;
            }
            else if (levelMusicSource.time >
                     (curRunnableModule.moveToNextTime + playerModel.maxAcceptableTimeThreshold))
            {
                JumpToNextRunnableModule();
                check = true;
            }

            check &= curRunnableModule.next;
        }
    }

    void CheckForPlayerJump()
    {
        if (curRunnableModule.autoRun)
        {
            return;
        }

        var playerInput = playerModel.GetPlayerInput();

        if (playerInput.isJumping)
        {
            bool jumpIsValid = false;
            if (transform.position.x > curRunnableModule.runXPos && playerInput.jumpRight)
            {
                jumpIsValid = true;
            }
            else if (transform.position.x < curRunnableModule.runXPos &&
                     playerInput.jumpLeft)
            {
                jumpIsValid = true;
            }

            if (jumpIsValid)
            {
                var jumpTimeOffset = Mathf.Abs(curRunnableModule.moveToNextTime - levelMusicSource.time);

                print("Jump Time Offset: " + jumpTimeOffset);

                if (jumpTimeOffset <= playerModel.perfectTimeThreshold)
                {
                    curRunnableModule.Perfect();
                    playerModel.JumpAttempted(PlayerModel.maxScore);
                }
                else if (jumpTimeOffset <= playerModel.maxAcceptableTimeThreshold)
                {
                    curRunnableModule.Acceptable();
                    playerModel.JumpAttempted(HelperUtilities.Remap(jumpTimeOffset, playerModel.perfectTimeThreshold,
                        playerModel.maxAcceptableTimeThreshold, PlayerModel.maxScore,
                        playerModel.maxAcceptableTimeScore));
                }
                else
                {
                    curRunnableModule.Fail();
                    playerModel.InvalidJumpAttempted();
                }
            }
            else
            {
                curRunnableModule.Fail();
                playerModel.InvalidJumpAttempted();
            }
        }
    }
}