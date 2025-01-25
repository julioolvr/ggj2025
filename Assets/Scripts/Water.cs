using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float initializationMovementSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 23)
        {
            transform.Translate(Vector3.right * initializationMovementSpeed * Time.deltaTime);
        }
        else
        {
            // GetComponent<AudioSource>().Stop();
        }
    }
}
