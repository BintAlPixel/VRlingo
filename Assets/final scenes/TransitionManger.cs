using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Canvas fadeCanvas;           // assign FadeCanvas (Screen Space - Camera)
    [SerializeField] private CanvasGroup fadeCanvasGroup; // assign Fader's CanvasGroup

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.6f;
    [SerializeField] private bool fadeInOnStart = true;
    [Tooltip("Removes any other 'Fader' CanvasGroups found in loaded scenes to avoid permanent black overlays.")]
    [SerializeField] private bool purgeSceneFaders = true;

    [Header("Robustness")]
    [Tooltip("How long to keep trying to bind to Camera.main after a load.")]
    [SerializeField] private float rebindCameraWindow = 1.0f;
    [Tooltip("If still black after load, force clear after this many seconds.")]
    [SerializeField] private float revealFailsafeDelay = 1.5f;

    public bool IsTransitioning { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            if (fadeCanvas) Destroy(fadeCanvas.gameObject);  // kill duplicate canvas to avoid black overlay
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (fadeCanvas) DontDestroyOnLoad(fadeCanvas.gameObject);

        if (fadeCanvasGroup)
        {
            fadeCanvasGroup.blocksRaycasts = true;
            fadeCanvasGroup.interactable = false;
            fadeCanvasGroup.alpha = 1f; // start black
        }

        RebindCanvasCamera(); // try once now
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    IEnumerator Start()
    {
        if (fadeInOnStart) yield return Fade(0f);
        else if (fadeCanvasGroup) fadeCanvasGroup.alpha = 0f;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Immediately try to rebind, then keep trying briefly in case XR camera appears a frame late.
        StartCoroutine(RebindCameraForAWhile());

        if (purgeSceneFaders) CleanupForeignFaders();

        // safety: if anything left us black, reveal soon
        StartCoroutine(FailsafeReveal());
    }

    IEnumerator RebindCameraForAWhile()
    {
        float end = Time.unscaledTime + rebindCameraWindow;
        while (Time.unscaledTime < end)
        {
            RebindCanvasCamera();
            if (fadeCanvas && fadeCanvas.worldCamera != null) yield break;
            yield return null;
        }
        // one last attempt
        RebindCanvasCamera();
    }

    void RebindCanvasCamera()
    {
        if (!fadeCanvas) return;
        var cam = Camera.main; // Make sure your XR camera in EVERY scene is tagged MainCamera
        if (cam != null)
        {
            fadeCanvas.worldCamera = cam;
            fadeCanvas.planeDistance = 0.5f;
            fadeCanvas.sortingOrder = 9999;
        }
    }

    void CleanupForeignFaders()
    {
        var allCgs = FindObjectsOfType<CanvasGroup>(true);
        foreach (var cg in allCgs)
        {
            if (!cg || cg == fadeCanvasGroup) continue;
            bool nameLooksLikeFader = cg.name.ToLower().Contains("fader");
            var c = cg.GetComponentInParent<Canvas>();
            bool looksOnTop = c && c.sortingOrder >= 9999;
            if (nameLooksLikeFader || (looksOnTop && cg.alpha >= 0.99f))
                cg.gameObject.SetActive(false);
        }
    }

    IEnumerator FailsafeReveal()
    {
        yield return new WaitForSecondsRealtime(revealFailsafeDelay);
        if (fadeCanvasGroup && fadeCanvasGroup.alpha > 0f) fadeCanvasGroup.alpha = 0f;
    }

    public void LoadScene(string sceneName)
    {
        if (IsTransitioning) return; // guard against double triggers
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        IsTransitioning = true;

        // fade to black
        yield return Fade(1f);

        // async load
        var op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;

        // ensure we’re bound to the new camera
        yield return RebindCameraForAWhile();

        // fade in
        yield return Fade(0f);

        // double safety
        fadeCanvasGroup.alpha = 0f;
        IsTransitioning = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (!fadeCanvasGroup) yield break;

        float start = fadeCanvasGroup.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(start, targetAlpha, t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
    }

    public void ForceClearBlack()
    {
        if (fadeCanvasGroup) fadeCanvasGroup.alpha = 0f;
    }
}
