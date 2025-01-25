using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public GameObject bubblePrefab;  // Prefab of the bubble to spawn
    public BoxCollider spawnArea;    // Area within which bubbles will be spawned

    public DifficultyLevel difficulty = DifficultyLevel.Medium;

    private float spawnDelay;
    private Vector2 bubbleSizeRange;

    public UnityEvent<Bubble> OnBubbleSpawned;

    float progress = 0;
    public float difficultyMultiplier = 3;

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
    }

    void SetDifficultyParameters()
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                spawnDelay = Random.Range(1.5f, 3.0f);
                bubbleSizeRange = new Vector2(1.0f, 2.0f);
                break;
            case DifficultyLevel.Medium:
                spawnDelay = Random.Range(1.0f, 2.0f);
                bubbleSizeRange = new Vector2(0.8f, 1.5f);
                break;
            case DifficultyLevel.Hard:
                spawnDelay = Random.Range(0.5f, 1.2f);
                bubbleSizeRange = new Vector2(0.5f, 1.2f);
                break;
        }
    }

    IEnumerator SpawnBubbles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay / (1 + progress * difficultyMultiplier));

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

    Vector3 GetRandomPosition()
    {
        // Get the bounds of the BoxCollider
        Bounds bounds = spawnArea.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);
        float fixedY = spawnArea.bounds.center.y;  // Keep Y constant

        return new Vector3(randomX, fixedY, randomZ);
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
