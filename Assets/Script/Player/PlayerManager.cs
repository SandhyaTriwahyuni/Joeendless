using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
  public static bool gameover;
    public GameObject GameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;

    public static int numberOfscore;
    public Text ScoreText;
     
      void Start()
    {
        Time.timeScale = 1;
        gameover = false;
        isGameStarted = false;
        numberOfscore = 0;
}

    // Update is called once per frame
    void Update()
    {
        if (gameover)
        {
            GameOverPanel.SetActive(true);
            Time.timeScale = 0;
         }
        ScoreText.text = "Score:" + numberOfscore;
         if (SwipeManager.tap && !isGameStarted)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }
}
