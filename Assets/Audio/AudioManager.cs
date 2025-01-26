using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeDuration = 1.5f;

    public void ChangeAudioClip(AudioClip newClip)
    {
        StartCoroutine(FadeOutIn(newClip));
    }


    public void FadeOut()
    {
        StartCoroutine(FadeOutCor());
    }

    private IEnumerator FadeOutCor()
    {
        yield return StartCoroutine(FadeAudio(1.0f, 0.0f));
    }


    private IEnumerator FadeOutIn(AudioClip newClip)
    {
        // Fade Out
        yield return StartCoroutine(FadeAudio(1.0f, 0.0f));

        // Cambiar el clip de audio
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade In
        yield return StartCoroutine(FadeAudio(0.0f, 1.0f));
    }

    private IEnumerator FadeAudio(float startVolume, float endVolume)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = endVolume;
    }
}