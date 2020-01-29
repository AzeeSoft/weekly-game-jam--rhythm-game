using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
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
    [Range(0, 1)] public float maxAcceptableTimeScoreMultiplier = 0.8f;
    [Range(0, 1)] public float missedBeatScoreMultiplier = 0f;
    [Range(0, 1)] public float invalidJumpScoreMultiplier = 0.3f;

    public float maxAcceptableTimeScore => maxScore * maxAcceptableTimeScoreMultiplier;

    public const float maxScore = 1f;

    public float totalScore = 0;
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
    }

    public void JumpMissed()
    {
        JumpAttempted(maxScore * missedBeatScoreMultiplier, false);
    }

    public void InvalidJumpAttempted()
    {
        JumpAttempted(maxScore * invalidJumpScoreMultiplier, false);
    }

    public void JumpAttempted(float score, bool jump = true)
    {
        playerMovementController.curRunnableModule.jumpAttempted = true;

        totalScore += score;
        scoresCount++;

        if (jump)
        {
            playerMovementController.JumpToNextRunnableModule();
        }
    }

    public PlayerInput GetPlayerInput()
    {
        var playerInput = new PlayerInput
        {
            jumpLeft = Input.GetKeyDown(KeyCode.LeftArrow),
            jumpRight = Input.GetKeyDown(KeyCode.RightArrow)
        };

        foreach (var leanFinger in LeanTouch.Fingers)
        {
            if (leanFinger.Down && !leanFinger.StartedOverGui)
            {
                var normalizedScreenX = leanFinger.ScreenPosition.x / Screen.currentResolution.width;
                if (normalizedScreenX <= 0.5)
                {
                    playerInput.jumpLeft = true;
                }
                else
                {
                    playerInput.jumpRight = true;
                }
            }
        }

        return playerInput;
    }
}