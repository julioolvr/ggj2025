using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float sidewaysSpeed = 1f;
    public float lifetime = 5f;
    public float amplitude = 1.0f;
    public GameObject popEffectPrefab;
    float timer = 0;

    void Start()
    {
        Destroy(gameObject, lifetime);
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
        // Instantiate the pop effect at the bubble's position and rotation
        GameObject popEffect = Instantiate(popEffectPrefab, transform.position, transform.rotation);

        // Optionally, scale the effect to match the bubble size
        popEffect.transform.localScale = transform.localScale;

        // Destroy the effect after a short time
        Destroy(popEffect, 1f);

        // Destroy the bubble itself
        Destroy(gameObject);
    }
}
