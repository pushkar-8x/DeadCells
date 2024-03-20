using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_AnimationTrigger : MonoBehaviour
{
    public Enemy_Skeleton _skeleton => GetComponentInParent<Enemy_Skeleton>();

    public void OnAnimationFinished()
    {
        _skeleton.AnimationFinishTrigger();
    }
}
