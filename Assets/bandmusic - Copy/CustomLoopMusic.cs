using UnityEngine;

public class AutoLoopMusic : MonoBehaviour
{
    public AudioSource music;
    public float loopStartTime = 4f; // وقت بداية اللوب
    private bool firstPlay = true;

    void Start()
    {
        // أول تشغيل من البداية
        music.time = 0f;
        music.Play();
        firstPlay = false;
    }

    void Update()
    {
        if (music.isPlaying)
        {
            // إذا وصل نهاية المقطع
            if (music.time >= music.clip.length - 0.1f)
            {
                music.time = loopStartTime; // يبدأ اللوب من الثانية 4
                music.Play();
            }
        }
    }
}