using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public void OnQuickStart()
    {
        SceneManager.LoadScene("Level01");
    }

    public void OnHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnReturn()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
