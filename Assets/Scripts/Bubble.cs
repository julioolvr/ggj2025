using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bubble : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float sidewaysSpeed = 1f;
    public float lifetime = 5f;
    public float amplitude = 1.0f;
    public float timeToDestroy = 0.5f;
    public GameObject popEffectPrefab;
    float timer = 0;
    public Butterfly Butterfly;
    public MeshRenderer meshRenderer;
    public Collider colliderBubble;

    public UnityEvent onBubblePopped;
    public UnityEvent onBubbleDestroyed;

    void Start()
    {
        Destroy(gameObject, lifetime);
        Butterfly.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
        float sidewaysTranslation = MathF.Cos(timer * sidewaysSpeed) * amplitude * 0.005f;
        transform.Translate(Vector3.left * sidewaysTranslation);
    }

    void OnMouseDown()
    {
        PopBubble();
    }

    public void PopBubble()
    {
        onBubblePopped.Invoke();

        // Instantiate the pop effect at the bubble's position and rotation
        GameObject popEffect = Instantiate(popEffectPrefab, transform.position, transform.rotation);

        // Optionally, scale the effect to match the bubble size
        popEffect.transform.localScale = transform.localScale;

        // Destroy the effect after a short time
        Destroy(popEffect, 1f);
        Debug.Log("DESACTIVO MESH");
        Butterfly.GoAway();
        meshRenderer.enabled = false;
        colliderBubble.enabled = false;
        Destroy(gameObject, timeToDestroy);

        // Destroy the bubble itself
        //Destroy(gameObject);
    }

    public void DestroyBubble()
    {
        onBubbleDestroyed.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BubbleDestroyer"))
        {
            DestroyBubble();
        }
        else
        {
            PopBubble();
        }
    }
}
