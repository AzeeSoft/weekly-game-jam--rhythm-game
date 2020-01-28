using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NormalizedPositioner : MonoBehaviour
{
    [Range(0, 1)]
    public float position = 0;

    private RectTransform rectTransform;
    private RectTransform parentRectTrans;

    void InitAsNeeded()
    {
        if (!rectTransform)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (!parentRectTrans)
        {
            parentRectTrans = transform.parent.GetComponent<RectTransform>();
        }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        InitAsNeeded();
    }

    // Update is called once per frame
    protected void Update()
    {
        InitAsNeeded();

        print(parentRectTrans.rect);

        var newPos = transform.localPosition;
        newPos.x = HelperUtilities.Remap(position, 0, 1, -parentRectTrans.rect.width/2, parentRectTrans.rect.width/2);
        transform.localPosition = newPos;

        print(newPos);
    }
}
