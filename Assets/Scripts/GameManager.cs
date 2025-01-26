using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
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

    public AudioSource audioSource;
    public AudioClip tictac;
    public AudioClip EndGameAudio;

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

    float elapsed = 0f;

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        bool newSecond = false;
        if(elapsed>= 1)
        {
            elapsed = 0f;
            newSecond = true;
        }

        if (isPlaying)
        {
            timePlayed += Time.deltaTime;
            hudTimePlayedText.text = PlayTimeText(newSecond);
            hudScoreText.text = ScoreText();

            if (timePlayed >= timeLimit)
            {
                FinishGame();
            }

            spawner.SetProgress(Mathf.Pow(timePlayed / timeLimit, 2));
            //spawner.SetProgress(timePlayed / timeLimit);
        }
        else
        {
            hudScoreText.text = ScoreText() + "/" + Bubble.contButterflies;
        }
    }

    bool once = true;
    string PlayTimeText(bool newSecond)
    {
        float remainingSeconds = timeLimit - timePlayed;
        int minutes = (int)(remainingSeconds / 60);
        int seconds = (int)(remainingSeconds % 60);

        if(newSecond && minutes == 0 && (seconds == 1 || seconds == 2 || seconds == 3 || seconds == 4 || seconds == 5))
        {
            audioSource.PlayOneShot(tictac);
        }

        if(once && newSecond && minutes == 0 && seconds == 0)
        {
            audioSource.PlayOneShot(EndGameAudio);
            once = false;
        }

        string dev = $"{minutes}:{seconds:D2}";

        return dev;
    }

    string ScoreText()
    {
        return ""+score;
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


    public AudioManager audioManager;
    void RestartGame()
    {
        audioManager.FadeOut();
        Invoke(nameof(ReloadScene), 2f);
    }

    void ReloadScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Bubble.contButterflies = 0;

    }

    private void HandleBubbleSpawned(Bubble bubble)
    {
        //Debug.Log($"A bubble was spawned at {bubble.transform.position}");
        bubble.onBubblePopped.AddListener(IncreaseScore);
        bubble.onBubbleDestroyed.AddListener(DecreaseScore);
    }
}
