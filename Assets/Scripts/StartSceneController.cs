using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public AudioSource startAudio; // Assign the start audio in the Inspector

    void Start()
    {
        // Play the start audio on loop when the start scene loads
        if (startAudio != null)
        {
            startAudio.loop = true;
            startAudio.Play();
        }
    }

    // This method is linked to the Level 1 button's OnClick event
    public void LoadLevel1()
    {
        // Stop the start audio when loading the next scene
        if (startAudio != null && startAudio.isPlaying)
        {
            startAudio.Stop();
        }

        // Load the game scene
        SceneManager.LoadScene("GameScene"); // Replace with your actual game scene name
    }
}