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
    public UnityEvent onBubbleSplit;
    public UnityEvent onBubbleDestroyed;
    public AudioClip[] popSounds;
    private AudioSource audioSource;
    public bool butterflyBubble = true;
    public GameObject bubblePrefab;

    void Start()
    {
        Destroy(gameObject, lifetime);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;

        if (butterflyBubble)
            transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);

        DontDestroyOnLoad(gameObject);
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
        // TODO: Right click vs left click
        PopBubble(true);
    }

    public void PopBubble(bool split)
    {
        PlayRandomPopSound();

        // Instantiate the pop effect at the bubble's position and rotation
        GameObject popEffect = Instantiate(popEffectPrefab, transform.position, transform.rotation);

        // Optionally, scale the effect to match the bubble size
        popEffect.transform.localScale = transform.localScale;

        // Destroy the effect after a short time
        Destroy(popEffect, 1f);
        Debug.Log("DESACTIVO MESH");

        TMPro.TextMeshProUGUI bubbleText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

        if (bubbleText != null)
        {
            // Destroy text immediately
            Destroy(bubbleText);
        }

        if (split)
        {
            GameObject bubbleLeft = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
            bubbleLeft.transform.localScale = transform.localScale * 0.5f;
            bubbleLeft.GetComponent<Rigidbody>().velocity = Vector3.left * 2f;


            GameObject bubbleRight = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
            bubbleRight.transform.localScale = transform.localScale * 0.5f;
            bubbleRight.GetComponent<Rigidbody>().velocity = Vector3.right * 2f;

            onBubbleSplit.Invoke();

            if (Butterfly)
            {
                Destroy(Butterfly);
            }

            Destroy(gameObject, timeToDestroy);
        }
        else
        {
            onBubblePopped.Invoke();

            if (Butterfly)
            {
                Butterfly.GoAway();
            }

        }

        meshRenderer.enabled = false;
        colliderBubble.enabled = false;
        Destroy(gameObject, timeToDestroy);
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
            PopBubble(false);
        }
    }

    private void PlayRandomPopSound()
    {
        if (popSounds.Length > 0)
        {
            AudioClip randomClip = popSounds[UnityEngine.Random.Range(0, popSounds.Length)];
            audioSource.PlayOneShot(randomClip);
        }
    }


}
