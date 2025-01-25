using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    bool isPlaying = false;
    float timePlayed = 0f;
    int score = 0;
    [SerializeField] private TextMeshProUGUI hudTimePlayedText;
    [SerializeField] private TextMeshProUGUI hudScoreText;
    public Spawner spawner;
    public GameObject water;
    public int timeLimit = 60;

    public void StartGame()
    {
        isPlaying = true;
        water.SetActive(true);
    }

    void OnEnable()
    {
        spawner.OnBubbleSpawned.AddListener(HandleBubbleSpawned);
    }

    void OnDisable()
    {
        spawner.OnBubbleSpawned.RemoveListener(HandleBubbleSpawned);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timePlayed += Time.deltaTime;
            hudTimePlayedText.text = PlayTimeText();
            hudScoreText.text = ScoreText();

            if (timePlayed >= timeLimit)
            {
                FinishGame();
            }
        }
    }

    string PlayTimeText()
    {
        float remainingSeconds = timeLimit - timePlayed;
        int minutes = (int)(remainingSeconds / 60);
        int seconds = (int)(remainingSeconds % 60);

        return $"{minutes}:{seconds:D2}";
    }

    string ScoreText()
    {
        return $"Score: {score}";
    }

    private void IncreaseScore()
    {
        score += 1;
    }

    private void FinishGame()
    {
        isPlaying = false;
        water.SetActive(false);

        // TODO: Remove all bubbles, or disable weapon?
        // Show score and restart bubble
    }

    private void HandleBubbleSpawned(Bubble bubble)
    {
        Debug.Log($"A bubble was spawned at {bubble.transform.position}");
        bubble.onBubblePopped.AddListener(IncreaseScore);
    }
}
