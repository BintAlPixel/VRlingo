using UnityEngine;

public class LookAt : MonoBehaviour
{
    [Header("Character Reference")]
    public GameObject character;    // 🟢 Drag your character هنا

    [Header("Path Settings")]
    public Transform[] waypoints;   // نقاط الطريق (حط الكيوبات)
    public float moveSpeed = 2f;    // سرعة المشي

    private int currentWaypoint = 0;
    private Animator animator;

    void Start()
    {
        if (character == null)
        {
            Debug.LogError("❌ No character assigned in LookAt script!");
            return;
        }

        animator = character.GetComponent<Animator>();

        if (waypoints.Length == 0)
        {
            Debug.LogWarning("⚠️ No waypoints assigned!");
        }
    }

    void Update()
    {
        if (character == null || waypoints.Length == 0) return;

        Vector3 targetPos = waypoints[currentWaypoint].position;
        targetPos.y = character.transform.position.y; // تجاهل فرق الارتفاع

        float distance = Vector3.Distance(character.transform.position, targetPos);

        if (distance > 0.1f)
        {
            // تشغيل المشي
            if (animator != null)
                animator.SetBool("isWalking", true);

            // حركة مباشرة
            character.transform.position = Vector3.MoveTowards(
                character.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            // إيقاف المشي
            if (animator != null)
                animator.SetBool("isWalking", false);

            // لو وصل الكيوب الأول خلي الدوران 0
            if (currentWaypoint == 0)
            {
                character.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            // لو وصل الكيوب الثاني خلي الدوران 180
            else if (currentWaypoint == 1)
            {
                character.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            // انتقال للنقطة التالية
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
}
