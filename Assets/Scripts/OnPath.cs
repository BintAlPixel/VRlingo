using UnityEngine;

public class OnPath : MonoBehaviour
{
    public GameObject character;    // 🟢 اسحب هنا شخصيتك
    public Transform[] waypoints;   // نقاط الطريق
    public float moveSpeed = 2f;    
    

    private int currentWaypoint = 0;
    private Animator animator;

    void Start()
    {
        if (character == null)
        {
            Debug.LogError("❌ ما حطيت الشخصية في Inspector!");
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
        targetPos.y = character.transform.position.y; 

        float distance = Vector3.Distance(character.transform.position, targetPos);

        if (distance > 0.1f)
        {
            if (animator != null)
                animator.SetBool("isWalking", true);

          

            // الحركة
            character.transform.position = Vector3.MoveTowards(character.transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            if (animator != null)
                animator.SetBool("isWalking", false);

            if (currentWaypoint < waypoints.Length - 1)
            {
                currentWaypoint++;
            }
           
        }
    }
}
