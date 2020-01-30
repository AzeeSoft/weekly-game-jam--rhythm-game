using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoorUI : SingletonMonoBehaviour<DoorUI>
{
    public RectTransform leftDoor;
    public RectTransform rightDoor;
    public RectTransform leftDivider;
    public RectTransform rightDivider;

    public bool openOnStart = true;
    public float defaultDoorOpenCloseDuration = 2f;

    public Animator animator;

    new void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (openOnStart)
        {
            OpenDoors();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoors(float duration = -1, TweenCallback callback = null)
    {
        AnimateDoors(0.5f, 0, () =>
        {
            EnableDoors(true);
            AnimateDoors(0, duration, () =>
            {
                EnableDoors(false);
                callback?.Invoke();
            });
        });
    }

    public void CloseDoors(float duration = -1, TweenCallback callback = null)
    {
        AnimateDoors(0f, 0, () =>
        {
            EnableDoors(true);
            AnimateDoors(0.5f, duration, callback);
        });
    }

    public void HideDividers(float duration)
    {
        AnimateDividers(0, duration);
    }

    public void RevealDividers(float duration)
    {
        AnimateDividers(1, duration);
    }

    void AnimateDoors(float endScale, float duration, TweenCallback callback)
    {
        if (duration < 0)
        {
            duration = defaultDoorOpenCloseDuration;
        }

        //leftDoor.DOKill(true);
        //rightDoor.DOKill(true);

        var leftDoorTween = leftDoor.DOScaleX(endScale, duration);
        var rightDoorTween = rightDoor.DOScaleX(endScale, duration);

        var seq = DOTween.Sequence();
        seq.Insert(0, leftDoorTween);
        seq.Insert(0, rightDoorTween);
        seq.AppendCallback(callback);
        seq.Play();
    }

    void EnableDoors(bool enable)
    {
        leftDoor.gameObject.SetActive(enable);
        rightDoor.gameObject.SetActive(enable);
    }

    void AnimateDividers(float endScale, float duration)
    {
        leftDivider.DOKill();
        rightDivider.DOKill();

        var leftDividerTween = leftDivider.DOScaleY(endScale, duration);
        var rightDividerTween = rightDivider.DOScaleY(endScale, duration);

        var seq = DOTween.Sequence();
        seq.Insert(0, leftDividerTween);
        seq.Insert(0, rightDividerTween);
        seq.Play();
    }
}
