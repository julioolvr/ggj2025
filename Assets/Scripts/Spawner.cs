using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public GameObject bubblePrefab;  // Prefab of the bubble to spawn
    public GameObject bubbleTimeLow;  // Prefab of the bubble to spawn
    public GameObject bubbleTimeFast;  // Prefab of the bubble to spawn
    public BoxCollider spawnArea;    // Area within which bubbles will be spawned

    public DifficultyLevel difficulty;

    public float spawnDelay = 2.2f;
    private Vector2 bubbleSizeRange;

    public UnityEvent<Bubble> OnBubbleSpawned;

    float progress = 0;
    public float difficultyMultiplier = 3;

    public float timeToAppearTheLowBubble = 48;
    public float timeToAppearTheFastBubble = 27;

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    private void Start()
    {
        SetDifficultyParameters();
        StartCoroutine(SpawnBubbles());
        StartCoroutine(SpawnLowBubble());
        StartCoroutine(SpawnFastBubble());

    }

    void SetDifficultyParameters()
    {
        bubbleSizeRange = new Vector2(1f, 1.5f);
    }

    /*
     * Funcion antigua
    IEnumerator SpawnBubbles()
    {
        while (true)
        {
            //yield return new WaitForSeconds(spawnDelay / (1 + difficultyMultiplier * progress));

            // Randomize position within the box collider, keeping the same Y position
            Vector3 spawnPosition = GetRandomPosition();

            // Instantiate bubble
            GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

            OnBubbleSpawned?.Invoke(bubble.GetComponent<Bubble>());

            // Randomize bubble size
            float randomSize = Random.Range(bubbleSizeRange.x, bubbleSizeRange.y);
            bubble.transform.localScale = Vector3.one * randomSize;

            // Adjust spawn delay dynamically to introduce randomness
            spawnDelay = Random.Range(spawnDelay * 0.8f, spawnDelay * 1.2f);
        }
    }
    */

    IEnumerator SpawnBubbles()
    {
        while (true)
        {
            float factorExponencial = -2.0f;

            // Definir los puntos clave de aceleración
            if (progress >= 0.83f)  // Últimos 10 segundos (50/60)
            {
                factorExponencial = -7.0f; //HACER MÁS NEGATIVO SI SE QUIERE MÁS RÁPIDO
            }
            else if (progress >= 0.58f)  // Desde los 35 segundos (35/60)
            {
                factorExponencial = -3.0f; //HACER MÁS NEGATIVO SI SE QUIERE MÁS RÁPIDO
            }
            else if (progress >= 0.33f)  // Desde los 20 segundos (20/60)
            {
                factorExponencial = -2.5f; //HACER MÁS NEGATIVO SI SE QUIERE MÁS RÁPIDO
            }

            // Aplica la nueva fórmula con los factores ajustados
            yield return new WaitForSeconds(spawnDelay / (1 + difficultyMultiplier * (1 - Mathf.Exp(factorExponencial * progress))));

            // Randomize position within the box collider, keeping the same Y position
            Vector3 spawnPosition = GetRandomPosition();

            // Instantiate bubble
            GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

            OnBubbleSpawned?.Invoke(bubble.GetComponent<Bubble>());

            // Randomize bubble size
            float randomSize = Random.Range(bubbleSizeRange.x, bubbleSizeRange.y);
            bubble.transform.localScale = Vector3.one * randomSize;

            // Adjust spawn delay dynamically to introduce randomness

            //spawnDelay = Random.Range(spawnDelay * 0.95f, spawnDelay * 1.1f);
        }
    }

    IEnumerator SpawnLowBubble()
    {
        yield return new WaitForSeconds(timeToAppearTheLowBubble);

        Vector3 spawnPosition = GetRandomPosition();
        GameObject bubble = Instantiate(bubbleTimeLow, spawnPosition, Quaternion.identity);
        bubble.SetActive(true);
        float randomSize = Random.Range(bubbleSizeRange.x, bubbleSizeRange.y);
        bubble.transform.localScale = Vector3.one * randomSize;
    }

    IEnumerator SpawnFastBubble()
    {
        yield return new WaitForSeconds(timeToAppearTheFastBubble);

        Vector3 spawnPosition = GetRandomPosition();
        GameObject bubble = Instantiate(bubbleTimeFast, spawnPosition, Quaternion.identity);
        bubble.SetActive(true);
        float randomSize = Random.Range(bubbleSizeRange.x, bubbleSizeRange.y);
        bubble.transform.localScale = Vector3.one * randomSize;
    }

    Vector3 GetRandomPosition()
    {
        // Get the bounds of the BoxCollider
        Bounds bounds = spawnArea.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(randomX, randomY, randomZ);
    }

    // Call this method to change difficulty dynamically
    public void ChangeDifficulty(DifficultyLevel newDifficulty)
    {
        difficulty = newDifficulty;
        SetDifficultyParameters();
    }

    public void SetProgress(float newProgress)
    {
        progress = newProgress;
    }
}
