using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static bool gameover;
    public GameObject GameOverPanel;

    public static bool isGameStarted;
    public GameObject startingText;

    public static int numberOfscore;
    public Text ScoreText;
    public Animator PlayerAnimator;

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
            StartCoroutine(GameOverSequence());
        }

        ScoreText.text = "Score: " + numberOfscore;

        if (SwipeManager.tap && !isGameStarted)
        {
            isGameStarted = true;
            Debug.Log("Game started! isGameStarted = true");
            Destroy(startingText);
        }

        Debug.Log("isGameStarted: " + isGameStarted);
    }
    IEnumerator GameOverSequence()
    {
        PlayerAnimator.SetTrigger("Die");
        // Menunggu hingga animasi "Die" selesai
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
    }
}
