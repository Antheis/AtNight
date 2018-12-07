using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScreen : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Level");
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
