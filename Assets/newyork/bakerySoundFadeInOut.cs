using UnityEngine;

public class BakeryAudioFade : MonoBehaviour
{
    public AudioSource bakeryAudio;
    public float fadeDuration = 2f; // مدة الفيدينج بالثواني
    private bool fading = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !fading)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInAudio());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !fading)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutAudio());
        }
    }

    System.Collections.IEnumerator FadeInAudio()
    {
        fading = true;
        bakeryAudio.Play();
        float elapsed = 0f;
        bakeryAudio.volume = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            bakeryAudio.volume = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        bakeryAudio.volume = 1f;
        fading = false;
    }

    System.Collections.IEnumerator FadeOutAudio()
    {
        fading = true;
        float startVolume = bakeryAudio.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            bakeryAudio.volume = Mathf.Clamp01(Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration));
            yield return null;
        }

        bakeryAudio.volume = 0f;
        bakeryAudio.Stop();
        fading = false;
    }
}