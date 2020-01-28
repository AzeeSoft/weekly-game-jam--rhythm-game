using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    public bool jumpLeft = false;
    public bool jumpRight = false;

    public bool isJumping => jumpLeft || jumpRight;
}

public class PlayerModel : SingletonMonoBehaviour<PlayerModel>
{
    public float accuracy => scoresCount > 0 ? totalScore / scoresCount : 0;

    private AudioSource levelMusicSource => LevelManager.Instance.levelMusicSource;
    public PlayerMovementController playerMovementController { get; private set; }

    public float playerSpeed = 1;

    [Header("Accuracy Config")] public float perfectTimeThreshold;
    public float maxAcceptableTimeThreshold;
    [Range(0, 1)] public float maxAcceptableTimeScoreMultiplier = 0.5f;

    public float maxAcceptableTimeScore => maxScore * maxAcceptableTimeScoreMultiplier;

    public const float maxScore = 1f;

    private float totalScore = 0;
    private int scoresCount = 0;

    new void Awake()
    {
        base.Awake();

        playerMovementController = GetComponent<PlayerMovementController>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var playerInput = GetPlayerInput();

        if (playerInput.isJumping)
        {
            if (!playerMovementController.curRunnableModule.jumpAttempted)
            {
                var jumpTimeOffset = playerMovementController.curRunnableModule.moveToNextTime - levelMusicSource.time;
                jumpTimeOffset = Mathf.Max(0, jumpTimeOffset);

                print("Jump Time Offset: " + jumpTimeOffset);

                if (jumpTimeOffset <= perfectTimeThreshold)
                {
                    JumpAttempted(maxScore);
                }
                else if (jumpTimeOffset <= maxAcceptableTimeThreshold)
                {
                    JumpAttempted(HelperUtilities.Remap(jumpTimeOffset, perfectTimeThreshold,
                        maxAcceptableTimeThreshold, maxScore, maxAcceptableTimeScore));
                }
                else
                {
                    InvalidJumpAttempted();
                }

                playerMovementController.curRunnableModule.jumpAttempted = true;
            }
            else
            {
                InvalidJumpAttempted();
            }
        }
    }

    public PlayerInput GetPlayerInput()
    {
        var playerInput = new PlayerInput
        {
            jumpLeft = Input.GetKeyDown(KeyCode.LeftArrow),
            jumpRight = Input.GetKeyDown(KeyCode.RightArrow)
        };

        return playerInput;
    }

    public void JumpMissed()
    {
        JumpAttempted(0);
    }

    public void InvalidJumpAttempted()
    {
        JumpAttempted(0);
    }

    public void JumpAttempted(float score)
    {
        totalScore += score;
        scoresCount++;
    }
}