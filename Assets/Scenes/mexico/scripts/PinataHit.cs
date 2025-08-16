using UnityEngine;

public class PinataHit : MonoBehaviour
{
    [Header("Objects that will fall when hit")]
    public GameObject[] objectsToFall;  // المجسمات اللي تبغين تسقط

    [Header("Force applied to falling objects")]
    public Vector3 fallForce = new Vector3(0, 2f, 2f); // دفعة للأوبجكتات عند الضربة

    void OnCollisionEnter(Collision collision)
    {
        // تحقق إذا الاصطدام جاء من المضرب
        if (collision.gameObject.CompareTag("Bat"))
        {
            // أضف قوة على البنياتا نفسها
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(collision.relativeVelocity * 2f, ForceMode.Impulse);
            }

            // تفعيل الأوبجكتات وسقوطها
            foreach (GameObject obj in objectsToFall)
            {
                if (obj != null)
                {
                    obj.SetActive(true); // يفعل المجسم لو كان مخفي
                    Rigidbody objRb = obj.GetComponent<Rigidbody>();
                    if (objRb != null)
                    {
                        objRb.isKinematic = false; // خلي الفيزياء تشتغل
                        objRb.AddForce(fallForce, ForceMode.Impulse); // دفعة للأوبجكت
                    }
                }
            }
        }
    }
}