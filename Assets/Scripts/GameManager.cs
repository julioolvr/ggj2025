using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TODO: Start as false and change it when shooting at "Start"
    bool isPlaying = true;
    float timePlayed = 0f;
    int score = 0;
    [SerializeField] private TextMeshProUGUI hudTimePlayedText;
    [SerializeField] private TextMeshProUGUI hudScoreText;
    public Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {

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
        }
    }

    string PlayTimeText()
    {
        int minutes = (int)(timePlayed / 60);
        int seconds = (int)(timePlayed % 60);

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

    private void HandleBubbleSpawned(Bubble bubble)
    {
        Debug.Log($"A bubble was spawned at {bubble.transform.position}");
        bubble.onBubblePopped.AddListener(IncreaseScore);
        // Additional logic for the spawned bubble (e.g., tracking, tagging, etc.)
    }
}
