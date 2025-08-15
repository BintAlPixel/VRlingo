using UnityEngine;

public class FaceCameraUI : MonoBehaviour
{
    public Transform cameraTransform;
    public float distance = 1.6f;
    public Vector3 offset = new Vector3(0, -0.05f, 0);
    public bool lockYRotation = true;

    void LateUpdate()
    {
        if (!cameraTransform) return;
        Vector3 target = cameraTransform.position + cameraTransform.forward * distance + offset;
        transform.position = target;

        Vector3 look = cameraTransform.position;
        if (lockYRotation) look.y = transform.position.y;
        transform.LookAt(look);
    }
}
