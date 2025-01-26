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
    public AudioClip[] popSounds;
    public AudioSource audioSource;
    public bool butterflyBubble = true;

    public static int contButterflies = 0;

    void Start()
    {
        StartCoroutine(DestroyBubbleAnim());
        //audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.spatialize = true;
        //audioSource.spatialBlend = .8f;

        if (butterflyBubble)
        {
            transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
            contButterflies++;
        }

        DontDestroyOnLoad(gameObject);

        floatSpeed = UnityEngine.Random.Range(floatSpeed, floatSpeed * 2f);

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
        PlayRandomPopSound();

        // Instantiate the pop effect at the bubble's position and rotation
        GameObject popEffect = Instantiate(popEffectPrefab, transform.position, transform.rotation);

        // Optionally, scale the effect to match the bubble size
        popEffect.transform.localScale = transform.localScale;

        // Destroy the effect after a short time
        Destroy(popEffect, 1f);

        if (Butterfly)
        {
            Butterfly.GoAway();
        }

        TMPro.TextMeshProUGUI bubbleText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

        if (bubbleText != null)
        {
            // Destroy text immediately
            Destroy(bubbleText);
        }

        meshRenderer.enabled = false;
        colliderBubble.enabled = false;
        Destroy(gameObject, timeToDestroy);

        // Destroy the bubble itself
        //Destroy(gameObject);
    }

    IEnumerator DestroyBubbleAnim()
    {
        yield return new WaitForSeconds(lifetime);

        float duration = 3;
        Vector3 initialScale = gameObject.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.localScale = targetScale;
        Destroy(gameObject);
    }

    public void DestroyBubble()
    {
        onBubbleDestroyed.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("lb_bird")) return;

        if (other.CompareTag("BubbleDestroyer"))
        {
            DestroyBubble();
        }
        else
        {
            PopBubble();
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
