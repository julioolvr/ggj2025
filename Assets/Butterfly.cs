using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [SerializeField] AnimationClip clipIdle;
    [SerializeField] AnimationClip clipGoAway;
    [SerializeField] Animation animationCmp;

    public void GoAway()
    {
        animationCmp.clip = clipGoAway;
        animationCmp.Play();
    }


}
