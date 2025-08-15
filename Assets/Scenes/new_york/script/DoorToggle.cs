using UnityEngine;

public class DoorToggle : MonoBehaviour
{
    public GameObject door;           // مجسم الباب
    public Collider doorCollider;     // الكولايدر حق الباب
    public AudioSource doorAudio;     // الأوديو اللي بيرتبط بالباب
    public float audioStartTime = 1.6f; // يبدأ الصوت من ثانيه 1.6
    public float openAngle = 90f;     // زاوية الفتح
    public float speed = 2f;          // سرعة الحركة
    public float autoCloseDelay = 3f; // كم ثانية قبل ما يتسكر الباب تلقائياً

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private float closeTimer = 0f;

    void Start()
    {
        closedRotation = door.transform.rotation;
        openRotation = Quaternion.Euler(door.transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            closeTimer = autoCloseDelay; // ابدأ عداد الإغلاق
            doorCollider.enabled = false;

            // تشغيل الصوت من الثانية المحددة
            if (doorAudio != null)
            {
                doorAudio.time = audioStartTime;
                doorAudio.Play();
            }
        }
        else
        {
            doorCollider.enabled = true;
        }
    }

    void Update()
    {
        // حركة الباب
        if (isOpen)
        {
            door.transform.rotation = Quaternion.Lerp(door.transform.rotation, openRotation, Time.deltaTime * speed);

            // عداد الإغلاق التلقائي
            if (closeTimer > 0f)
            {
                closeTimer -= Time.deltaTime;
                if (closeTimer <= 0f)
                {
                    isOpen = false;
                    doorCollider.enabled = true;
                }
            }
        }
        else
        {
            door.transform.rotation = Quaternion.Lerp(door.transform.rotation, closedRotation, Time.deltaTime * speed);
        }
    }
}