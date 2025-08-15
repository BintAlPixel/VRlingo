using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabPhysicsSwitch : MonoBehaviour
{
    private Rigidbody rb;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        // قبل المسك يكون ثابت
        rb.isKinematic = true;
        rb.useGravity = false;

        // ربط الأحداث
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        rb.isKinematic = false;  // يصير فيزيائي
        rb.useGravity = true;    // الجاذبية شغالة
    }

    void OnRelease(SelectExitEventArgs args)
    {
        rb.isKinematic = true;   // يرجع ثابت
        rb.useGravity = false;   // يوقف الجاذبية
    }
}