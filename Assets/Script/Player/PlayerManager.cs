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

    public Text highscoreText;  // Text untuk menampilkan highscore di panel GameOver
    public Text currentScoreText;  // Text untuk menampilkan skor saat ini di panel GameOver

    private int highscore;

    void Start()
    {
        Time.timeScale = 1;
        gameover = false;
        isGameStarted = false;
        numberOfscore = 0;

        // Load highscore dari PlayerPrefs (jika ada)
        highscore = PlayerPrefs.GetInt("Highscore", 0);
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

        // Cek jika skor saat ini lebih tinggi dari highscore, dan simpan highscore baru
        if (numberOfscore > highscore)
        {
            highscore = numberOfscore;
            PlayerPrefs.SetInt("Highscore", highscore);  // Simpan highscore di PlayerPrefs
            PlayerPrefs.Save();
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

        // Menampilkan skor dan highscore di panel GameOver
        currentScoreText.text = "Score: " + numberOfscore;
        highscoreText.text = "Highscore: " + highscore;
    }
}
