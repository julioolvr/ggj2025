using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isPlaying = false;
    float timePlayed = 0f;
    int score = 0;
    [SerializeField] private TextMeshProUGUI hudTimePlayedText;
    [SerializeField] private TextMeshProUGUI hudScoreText;
    public Spawner spawner;
    public int timeLimit = 60;
    public UnityEvent onGameStart;
    public UnityEvent onGameOver;
    public GameObject bubbleRestartPrefab;
    public GameObject playerPlatform;

    public void StartGame()
    {
        isPlaying = true;
        onGameStart?.Invoke();
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

    private void DecreaseScore()
    {
        score -= 1;
    }

    private void FinishGame()
    {
        isPlaying = false;
        onGameOver?.Invoke();

        foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("GameBubble"))
        {
            Destroy(bubble);
        }

        Invoke(nameof(RestartBubble), 2f);
    }

    void RestartBubble()
    {
        GameObject restartBubble = Instantiate(bubbleRestartPrefab, playerPlatform.transform.position, Quaternion.identity);
        restartBubble.GetComponent<Bubble>().onBubblePopped.AddListener(RestartGame);
    }

    void RestartGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private void HandleBubbleSpawned(Bubble bubble)
    {
        Debug.Log($"A bubble was spawned at {bubble.transform.position}");
        bubble.onBubblePopped.AddListener(IncreaseScore);
        bubble.onBubbleDestroyed.AddListener(DecreaseScore);
    }
}
