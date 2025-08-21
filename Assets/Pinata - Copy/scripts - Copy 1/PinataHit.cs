using System.Collections.Generic;
using UnityEngine;

public class PinataHit : MonoBehaviour
{
    [Header("Candy Objects")]
    public List<GameObject> candies;   // لستة الكانديز (خليهم في البداية SetActive = false)

    [Header("Audio")]
    public AudioSource hitSound;       // صوت الضربة

    [Header("Pinata Settings")]
    public GameObject pinataObject;    // مجسم البنياتا نفسه
    private int hitCount = 0;          // عدد الضربات

    void OnCollisionEnter(Collision collision)
    {
        // نتأكد إنه المضرب (مثلاً تاق "Bat")
        if (collision.gameObject.CompareTag("Bat"))
        {
            HandleHit();
        }
    }

    void HandleHit()
    {
        // نشغل الصوت
        if (hitSound != null)
            hitSound.Play();

        // إذا باقي عندنا كانديز نخلي واحد يطيح
        if (hitCount < candies.Count)
        {
            candies[hitCount].SetActive(true);  // نخليه يبان
            hitCount++;
        }
        else
        {
            // إذا خلصوا الكانديز → نكسر/نخفي البنياتا
            EndPinata();
        }
    }

  void EndPinata()
{
    if (pinataObject != null)
    {
        // نشغل سكربت التأثير النهائي
        PinataEndEffect endEffect = pinataObject.GetComponent<PinataEndEffect>();
        if (endEffect != null)
            endEffect.PlayEndEffect();
    }

    Debug.Log("Pinata Finished! 🎉");
}
}