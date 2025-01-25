using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCustom : MonoBehaviour
{
    void Start()
    {
    }

    [ContextMenu("Restart Level")]
    public void Restart() { 

        OVRScreenFade.instance.FadeIn();
        
    }

}
