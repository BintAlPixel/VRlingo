using UnityEngine;

public class PinataSwing : MonoBehaviour
{
    public float initialForce = 3f;  // قوة البداية
    public float randomForce = 1f;   // قوة هواء بسيطة

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // دفعة أولية عشان تبدأ تتحرك
        rb.AddForce(new Vector3(initialForce, 0, 0), ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        // هواء بسيط يخليها ما توقف
        float x = Mathf.Sin(Time.time * 0.5f) * randomForce;
        rb.AddForce(new Vector3(x, 0, 0));
    }
}