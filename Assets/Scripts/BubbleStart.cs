using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BubbleStart : MonoBehaviour
{
    public GameObject Spawner;

    public UnityEvent onBubblePopped;

    private void OnTriggerEnter(Collider other)
    {
        onBubblePopped?.Invoke();

        if (Spawner)
        {
            Spawner.SetActive(true);
        }
        Destroy(transform.gameObject);
    }

    void OnMouseDown()
    {
        onBubblePopped?.Invoke();

        if (Spawner)
        {
            Spawner.SetActive(true);
        }

        Destroy(transform.gameObject);
    }
}
