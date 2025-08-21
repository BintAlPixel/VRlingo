using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TicketSocketLoader : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;
    [SerializeField] private float holdDelay = 0.75f; // must stay snapped this long
    private bool isProcessing;

    void OnEnable()
    {
        if (!socket) socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        socket.selectEntered.AddListener(OnSelectEntered);
        socket.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        if (!socket) return;
        socket.selectEntered.RemoveListener(OnSelectEntered);
        socket.selectExited.RemoveListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (isProcessing) return;

        // NEW: don't react if a transition is already running
        if (TransitionManager.Instance && TransitionManager.Instance.IsTransitioning) return; // NEW

        var ticket = args.interactableObject.transform.GetComponent<TicketItem>();
        if (ticket == null || ticket.data == null) return;

        StartCoroutine(LoadAfterDelay(ticket.data.sceneName));
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        isProcessing = false;
        StopAllCoroutines(); // cancel if pulled out early
    }

    IEnumerator LoadAfterDelay(string sceneName)
    {
        isProcessing = true;

        // small confirmation delay to avoid accidental touches
        float t = 0f;
        while (t < holdDelay)
        {
            // NEW: if a transition starts while waiting, bail
            if (TransitionManager.Instance && TransitionManager.Instance.IsTransitioning)  // NEW
            {
                isProcessing = false;                                                    // NEW
                yield break;                                                             // NEW
            }

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        if (TransitionManager.Instance == null)
        {
            Debug.LogError("TransitionManager missing. Put it in your starting scene.");
            isProcessing = false;
            yield break;
        }

        // NEW: final guard right before triggering the fade/load
        if (TransitionManager.Instance.IsTransitioning)                                   // NEW
        {
            isProcessing = false;                                                         // NEW
            yield break;                                                                  // NEW
        }

        TransitionManager.Instance.LoadScene(sceneName); // fade → load → fade-in
    }
}
