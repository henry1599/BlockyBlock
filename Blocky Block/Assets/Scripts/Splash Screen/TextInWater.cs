using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextInWater : MonoBehaviour
{
    public float Delay;
    public float TweenDuration;
    public float MinFloatingHeightBelow, MaxFloatingHeightBelow;
    void Start()
    {
        float floatingHeightBelow = Random.Range(MinFloatingHeightBelow, MaxFloatingHeightBelow);
        DOVirtual.DelayedCall(Delay, () => {
            transform.DOMoveY(floatingHeightBelow, TweenDuration);
            // .SetLoops(-1, LoopType.Yoyo);
        });
    }
}
