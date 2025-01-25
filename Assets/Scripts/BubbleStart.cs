using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BubbleStart : MonoBehaviour
{
    public GameObject Spawner;

    public float floatSpeed = 1f;  // Velocidad de movimiento vertical
    public float floatHeight = 0.5f;  // Altura m�xima a la que sube y baja
    private Vector3 startPos;

    public UnityEvent onBubblePopped;

    void Start()
    {
        startPos = transform.position;  // Guardamos la posici�n inicial
    }

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

    void Update()
    {
        // Movimiento suave usando una onda senoidal para arriba y abajo
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
