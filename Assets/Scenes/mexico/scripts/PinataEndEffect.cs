using UnityEngine;
using System.Collections;

public class PinataEndEffect : MonoBehaviour
{
    [Header("Pinata Parts")]
    public Transform[] pinataParts;

    [Header("Shake Settings")]
    public float shakeDuration = 0.3f;   // أقل من قبل
    public float shakeAmount = 0.02f;    // أخف

    [Header("Drift Settings")]
    public float driftForce = 0.1f;      
    public float torqueForce = 0.05f;    
    public float driftDuration = 1.0f;   

    [Header("Fade Out Settings")]
    public float fadeDuration = 1.5f;

    private bool isExploding = false;

    public void PlayEndEffect()
    {
        if (!isExploding)
            StartCoroutine(EndEffectRoutine());
    }

    IEnumerator EndEffectRoutine()
    {
        isExploding = true;

        // ✅ اهتزاز قصير للParent فقط (بعد انتهاء كل الكانديز)
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;

        // ✅ تفعيل Rigidbody للأجزاء فقط الآن
        foreach (Transform part in pinataParts)
        {
            Rigidbody rb = part.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                Vector3 randomDrift = Random.insideUnitSphere * driftForce;
                Vector3 randomTorque = Random.insideUnitSphere * torqueForce;

                rb.AddForce(randomDrift, ForceMode.Impulse);
                rb.AddTorque(randomTorque, ForceMode.Impulse);
            }
        }

        // ✅ فترة حركة قصيرة قبل الـFade
        yield return new WaitForSeconds(driftDuration);

        // ✅ Fade Out تدريجي
        float fadeElapsed = 0f;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        while (fadeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, fadeElapsed / fadeDuration);
            foreach (Renderer rend in renderers)
            {
                if (rend.material.HasProperty("_Color"))
                {
                    Color c = rend.material.color;
                    c.a = alpha;
                    rend.material.color = c;
                }
            }
            fadeElapsed += Time.deltaTime;
            yield return null;
        }

        // ✅ تدمير نهائي للأجزاء
        foreach (Transform part in pinataParts)
            Destroy(part.gameObject);

        Destroy(gameObject);
    }
}