using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private AudioSource levelMusicSource => LevelManager.Instance.levelMusicSource;
    private float playerSpeed => playerModel.playerSpeed;

    public bool autoSwitch = false;
    public float lerpSpeed = 1f;
    public float reachDistance = 0.2f;
    public Transform feetRef;

    private PlayerModel playerModel;
    private RunnableModule curRunnableModule;

    void Awake()
    {
        playerModel = GetComponent<PlayerModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        curRunnableModule = LevelManager.Instance.allRunnableModules[0];
        print(curRunnableModule);
        print(curRunnableModule.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        CheckAndUpdateRunnableModule();
        Move();
    }

    void CheckAndUpdateRunnableModule()
    {
        if (autoSwitch)
        {
            while (curRunnableModule.next && levelMusicSource.time > curRunnableModule.moveToNextTime)
            {
                curRunnableModule = curRunnableModule.next;
            }
        }
    }

    void Move()
    {
        var adjustedXPos = curRunnableModule.runXPos + (transform.position.x > curRunnableModule.runXPos ? 1 : -1) *
                           Mathf.Abs(transform.position.x - feetRef.position.x);

        var targetPos =
            new Vector3(Mathf.Lerp(transform.position.x, adjustedXPos, Time.deltaTime * lerpSpeed),
                levelMusicSource.time * playerSpeed);
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
}