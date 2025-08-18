using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProximityMusic : MonoBehaviour
{
    public Transform listener;      // كاميرا اللاعب
    public float minDistance = 5f;  // صوت كامل من هالمدى وأقرب
    public float maxDistance = 50f; // ينعدم بعده
    public bool playOnStart = true;

    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        src.spatialBlend = 1f;
        src.dopplerLevel = 0f;
        src.rolloffMode = AudioRolloffMode.Linear; // بنتحكم يدويًا
        src.minDistance = 1f;
        src.maxDistance = maxDistance;
    }

    void Start()
    {
        if (!listener) 
            listener = Camera.main ? Camera.main.transform : null;

        if (playOnStart && !src.isPlaying) src.Play();
    }

    void Update()
    {
        if (!listener) return;

        float d = Vector3.Distance(listener.position, transform.position);
        // 0 بعيد جدًا → 1 قريب
        float t = Mathf.InverseLerp(maxDistance, minDistance, d);
        // منحنى خفيف للنعومة
        float vol = Mathf.SmoothStep(0f, 1f, t);
        src.volume = vol;
    }
}