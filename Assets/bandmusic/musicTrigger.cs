using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioSource music; // حطي فيه Audio Source من الستيج
    public Transform player;  // الكاميرا أو XR Rig

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            if (!music.isPlaying)
                music.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            music.Stop(); // أو music.Pause() إذا تبغين تكمل من نفس المكان
        }
    }
}