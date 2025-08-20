using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhoneScreenController : MonoBehaviour
{
    [Header("References")]
    public Image blackScreen;      // BlackScreen Image (سوداء)
    public Image myUI;             // الصورة على شاشة الجوال (myUI.png)
    public GameObject buttonsPanel; // Panel فيه الأزرار

    [Header("Timing")]
    public float fadeDelay = 1f;   // وقت انتظار قبل ظهور UI
    public float fadeDuration = 0.5f; // مدة الفيد

    void Start()
    {
        // البداية
        if (blackScreen) blackScreen.gameObject.SetActive(true);
        if (myUI) myUI.gameObject.SetActive(false);
        if (buttonsPanel) buttonsPanel.SetActive(false);
    }

    // ينادى لما اللاعب يأخذ الجوال من السوكيت
    public void OnPhoneTaken()
    {
        StartCoroutine(TakeFlow());
    }

    // ينادى لما يرجع الجوال للجيب
    public void OnPhoneStored()
    {
        // إعادة تعيين الحالة: شاشة سوداء + إخفاء UI
        if (blackScreen) blackScreen.gameObject.SetActive(true);
        if (myUI) myUI.gameObject.SetActive(false);
        if (buttonsPanel) buttonsPanel.SetActive(false);
    }

    IEnumerator TakeFlow()
    {
        // فيد دخول الشاشة السوداء
        if (blackScreen) yield return StartCoroutine(FadeImage(blackScreen, 0f, 1f, fadeDuration));

        // إخفاء UI بالبداية
        if (myUI) myUI.gameObject.SetActive(false);
        if (buttonsPanel) buttonsPanel.SetActive(false);

        // انتظر fadeDelay ثانية
        yield return new WaitForSeconds(fadeDelay);

        // شغل الصورة + الأزرار
        if (myUI) myUI.gameObject.SetActive(true);
        if (buttonsPanel) buttonsPanel.SetActive(true);

        // فيد خروج السواد
        if (blackScreen) yield return StartCoroutine(FadeImage(blackScreen, 1f, 0f, fadeDuration));
    }

    IEnumerator FadeImage(Image img, float fromAlpha, float toAlpha, float duration)
    {
        float t = 0f;
        Color c = img.color;
        c.a = fromAlpha;
        img.color = c;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(fromAlpha, toAlpha, t / duration);
            img.color = c;
            yield return null;
        }

        c.a = toAlpha;
        img.color = c;
    }
}