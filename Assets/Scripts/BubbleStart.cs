using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleStart : MonoBehaviour
{
    public GameObject Spawner;

    public float floatSpeed = 1f;  // Velocidad de movimiento vertical
    public float floatHeight = 0.5f;  // Altura máxima a la que sube y baja
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;  // Guardamos la posición inicial
    }

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

    void Update()
    {
        // Movimiento suave usando una onda senoidal para arriba y abajo
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
