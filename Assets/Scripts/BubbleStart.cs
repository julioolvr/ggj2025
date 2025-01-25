using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleStart : MonoBehaviour
{
    public GameObject Spawner;

    private void OnTriggerEnter(Collider other)
    {
        Spawner.SetActive(true);
        Destroy(transform.gameObject);
    }

    void OnMouseDown()
    {
        Spawner.SetActive(true);
        Destroy(transform.gameObject);
    }
}
