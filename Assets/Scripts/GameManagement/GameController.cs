using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static int points = 0;
    public static AudioClip[] gameOverSounds;
    public AudioClip gameOverMusic;
    public static GameObject raycastPlane;
    public static bool gameOver = false;
    public static GameObject player;

    private static Text pointText;
    private static Text inGamePointText;

    private static bool showDeathScreen;
    private static bool showWinScreen;

    private static GameObject deathScreen;
    private static GameObject winScreen;

    public GameObject pauseScreen;

    bool paused = false;

    const float normalTimeScale = 1;

    public static void ChangePoints(int amount)
    {
        points += amount;
        inGamePointText.text = points.ToString();
    }

    void Start()
    {
        deathScreen = GameObject.Find("DeathScreen");
        winScreen = GameObject.Find("WinScreen");
        inGamePointText = GameObject.Find("InGamePointText").GetComponent<Text>();

        pointText = winScreen.transform.GetChild(0).GetComponent<Text>();
        

        deathScreen.SetActive(false);
        winScreen.SetActive(false);

        showDeathScreen = false;
        showWinScreen = false;
    }

    void OnLevelWasLoaded()
    {
        Time.timeScale = normalTimeScale;
    }

    void Update()
    {
        if (showDeathScreen)
            deathScreen.GetComponent<CanvasGroup>().alpha += .005f;

        if (showWinScreen)
            winScreen.GetComponent<CanvasGroup>().alpha += .005f;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        paused = !paused;

        if (paused) 
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            AudioPlayer.FadeOutMusic(GetComponent<AudioSource>());
        }
        else{
            Time.timeScale = normalTimeScale;
            pauseScreen.SetActive(false);
            AudioPlayer.FadeInMusic(GetComponent<AudioSource>());
        }
    }

    public void GoToMainMenu()
    {
        Application.LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }


    public static void FadeInDeathScreen()
    {
        deathScreen.SetActive(true);
        showDeathScreen = true;
    }
    public static void FadeInWinScreen()
    {
        winScreen.SetActive(true);
        showWinScreen = true;
    }

    public static void WinGame()
    {
        FadeInWinScreen();
        player.GetComponent<PlayerStats>().EnableInvulnurability(); 
        AudioPlayer.Stop();
        AudioPlayer.PlayWinClip();
        pointText.text = "points: " + points;
    }

    public static void EndGame()
    {
        FadeInDeathScreen();
        gameOver = true; 
        AudioPlayer.Stop();
        AudioPlayer.PlayLoseClip();
        //FadeTool.BeginFade(1);
        //GameEndDelay();
        //AudioPlayer.FadeOutMusic(AudioPlayer.music);        
    }

    public static IEnumerator GameEndDelay()
    {
        yield return new WaitForSeconds(3f);
        //AudioClip temp = gameOverSounds[Random.Range(0, gameOverSounds.Length)];
        //AudioPlayer.Play2DSound(temp);
        ////Invoke("GameEndSynth", temp.length);
    }

    void GameEndSynth()
    {
        //AudioPlayer.Play2DSound(gameOverMusic);
    }
}