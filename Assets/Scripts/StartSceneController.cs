using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public AudioSource startAudio; 

    void Start()
    {
        if (startAudio != null)
        {
            startAudio.loop = true;
            startAudio.Play();
        }
    }
    
    public void LoadLevel1()
    {
        if (startAudio != null && startAudio.isPlaying)
        {
            startAudio.Stop();
        }
        SceneManager.LoadScene("Game");
    }
}