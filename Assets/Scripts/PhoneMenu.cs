using UnityEngine;
using UnityEngine.SceneManagement;

public class PhoneMenu : MonoBehaviour
{
    public void Go()
    {
        SceneManager.LoadScene("airport main"); // <- use your exact scene name
    }
}
