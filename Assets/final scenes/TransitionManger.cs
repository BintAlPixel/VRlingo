using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Canvas fadeCanvas;           // ← assign FadeCanvas (Canvas component)
    [SerializeField] private CanvasGroup fadeCanvasGroup; // ← assign Fader's CanvasGroup

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.6f;
    [SerializeField] private bool fadeInOnStart = true;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Persist manager and canvas across scenes
        DontDestroyOnLoad(gameObject);
        if (fadeCanvas) DontDestroyOnLoad(fadeCanvas.gameObject);

        // Start black; we’ll fade in on Start
        if (fadeCanvasGroup)
        {
            fadeCanvasGroup.blocksRaycasts = true; // blocks UI rays during fade
            fadeCanvasGroup.interactable = false;
            fadeCanvasGroup.alpha = 1f;
        }

        // bind to current camera
        RebindCanvasCamera();

        // rebind after each load (new scene has a different MainCamera)
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindCanvasCamera();
    }

    void RebindCanvasCamera()
    {
        if (!fadeCanvas) return;
        var cam = Camera.main;
        if (cam != null)
        {
            fadeCanvas.worldCamera = cam;
            // keep canvas in front of camera
            fadeCanvas.planeDistance = 0.5f;
            fadeCanvas.sortingOrder = 9999;
        }
    }

    IEnumerator Start()
    {
        if (fadeInOnStart) yield return Fade(0f);
        else if (fadeCanvasGroup) fadeCanvasGroup.alpha = 0f;
    }

    public void LoadScene(string sceneName) => StartCoroutine(LoadSceneRoutine(sceneName));

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // Fade to black
        yield return Fade(1f);

        // Load scene async
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;

        // Rebind camera (new scene)
        RebindCanvasCamera();

        // Fade into the new scene
        yield return Fade(0f);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (!fadeCanvasGroup) yield break;

        float start = fadeCanvasGroup.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // unaffected by timescale
            fadeCanvasGroup.alpha = Mathf.Lerp(start, targetAlpha, t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
    }
}
