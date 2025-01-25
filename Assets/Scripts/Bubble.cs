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
}
