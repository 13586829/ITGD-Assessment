using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButtonController : MonoBehaviour
{
    public void ExitToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}