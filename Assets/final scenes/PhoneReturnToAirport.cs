using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PhoneReturnToAirport_XRI : MonoBehaviour
{
    [SerializeField] private string airportSceneName = "Airport"; // set in Inspector
    private XRGrabInteractable grab;
    private bool fired;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        if (!grab) Debug.LogError("XRGrabInteractable required on the phone.");
    }

    void OnEnable()
    {
        if (grab) grab.activated.AddListener(OnActivated); // Trigger = Activate by default
    }

    void OnDisable()
    {
        if (grab) grab.activated.RemoveListener(OnActivated);
    }

    private void OnActivated(ActivateEventArgs _)
    {
        if (fired) return; // prevent double-fire
        fired = true;

        if (TransitionManager.Instance != null)
            TransitionManager.Instance.LoadScene(airportSceneName); // fade → load → fade-in
        else
            SceneManager.LoadScene(airportSceneName); // fallback (no fade)
    }
}
