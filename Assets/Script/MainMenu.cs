using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI HighscoreText;

    void Start()
    {
        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        HighscoreText.text = "HIGHSCORE :" + highscore;
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("Level");    
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
