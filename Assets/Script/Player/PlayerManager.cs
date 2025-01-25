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
        gameover = false;
        Time.timeScale = 1;
        isGameStarted = false;
        numberOfscore = 0;
}

    // Update is called once per frame
    void Update()
    {
        if (gameover)
        {
            Time.timeScale = 0;
            GameOverPanel.SetActive(true);
        }
        ScoreText.text = "SCORE:" + numberOfscore;
        



        if (SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(startingText);
        }
    }
}
