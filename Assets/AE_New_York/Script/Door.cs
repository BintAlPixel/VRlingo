using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorXR : MonoBehaviour
{
    public float openAngle = 90f;      // زاوية فتح الباب
    public float smooth = 2f;          // سرعة الفتح
    public float closeDelay = 2f;      // ثواني قبل الإغلاق
    private Vector3 closedRot;
    private Vector3 openRot;
    private bool isMoving = false;

    void Start()
    {
        closedRot = transform.eulerAngles;
        openRot = new Vector3(closedRot.x, closedRot.y + openAngle, closedRot.z);
    }

    // هذه الدالة تربطها مع XR Interaction Toolkit
    // مثلاً: OnHoverEntered أو OnSelectEntered
    public void OpenDoor()
    {
        if (!isMoving)
            StartCoroutine(DoorRoutine());
    }

    private IEnumerator DoorRoutine()
    {
        isMoving = true;

        // فتح الباب تدريجيًا
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * smooth * 0.5f;
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, t);
            yield return null;
        }

        // الانتظار قبل الإغلاق
        yield return new WaitForSeconds(closeDelay);

        // غلق الباب تدريجيًا
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * smooth * 0.5f;
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, closedRot, t);
            yield return null;
        }

        isMoving = false;
    }
}