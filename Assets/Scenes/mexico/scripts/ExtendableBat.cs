using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ExtendableBat : MonoBehaviour
{
    [Header("XR Setup")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;    // المضرب نفسه
    public Transform extendedPosition;            // مكان اليد أو Right Controller
    public GameObject rightHandVisual;            // نموذج اليد اللي يختفي أثناء الإمساك

    [Header("Settings")]
    public float followSpeed = 10f;               // سرعة الحركة عند الإمساك

    private bool isGrabbed = false;

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        // أخفي اليد
        if (rightHandVisual != null)
            rightHandVisual.SetActive(false);

        // ضع المضرب مباشرة عند مكان اليد
        transform.position = extendedPosition.position;
        transform.rotation = extendedPosition.rotation;

        StartCoroutine(FollowHand());
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;

        // أظهر اليد مرة ثانية
        if (rightHandVisual != null)
            rightHandVisual.SetActive(true);
    }

    IEnumerator FollowHand()
    {
        while (isGrabbed)
        {
            transform.position = Vector3.Lerp(transform.position, extendedPosition.position, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, extendedPosition.rotation, Time.deltaTime * followSpeed);
            yield return null;
        }
    }
}