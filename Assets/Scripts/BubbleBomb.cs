using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BubbleBomb : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float sidewaysSpeed = 1f;
    public float lifetime = 5f;
    public float amplitude = 1.0f;
    public float timeToDestroy = 0.5f;
    public GameObject popEffectPrefab;
    float timer = 0;
    public MeshRenderer meshRenderer;
    public Collider colliderBubble;
    public UnityEvent onBubblePopped;
    public UnityEvent onBubbleDestroyed;
    public float durationOfEffect;
    public AudioSource specialClipPop;

    public float newtimeScale = 1f;

    void Start()
    {
        StartCoroutine(DestroyBubbleAnim());

        DontDestroyOnLoad(gameObject);


    }

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
        PlayPopSound();

        // Instantiate the pop effect at the bubble's position and rotation
        GameObject popEffect = Instantiate(popEffectPrefab, transform.position, transform.rotation);

        // Optionally, scale the effect to match the bubble size
        popEffect.transform.localScale = transform.localScale;

        // Destroy the effect after a short time
        Destroy(popEffect, 1f);


        TMPro.TextMeshProUGUI bubbleText = GetComponentInChildren<TMPro.TextMeshProUGUI>();

        if (bubbleText != null)
        {
            // Destroy text immediately
            Destroy(bubbleText);
        }

        meshRenderer.enabled = false;
        colliderBubble.enabled = false;
        Destroy(gameObject, timeToDestroy);


        StartCoroutine(ApplyEffect());
        // Destroy the bubble itself
        //Destroy(gameObject);
    }

    IEnumerator ApplyEffect()
    {
        Time.timeScale = newtimeScale;
        yield return new WaitForSeconds(durationOfEffect);
        Time.timeScale = 1f;

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

    private void PlayPopSound()
    {
        specialClipPop.Play();
    }
}
