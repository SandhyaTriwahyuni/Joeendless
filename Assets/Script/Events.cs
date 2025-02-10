using UnityEngine.SceneManagement;
using UnityEngine;

public class Events : MonoBehaviour
{
    public void ReplayGame() 
    {
        SceneManager.LoadScene("Level");
    }

    public void BACK()
    {
        SceneManager.LoadScene("Menu");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
