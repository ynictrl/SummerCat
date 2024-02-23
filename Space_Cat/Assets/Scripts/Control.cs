using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Control : MonoBehaviour
{
    [Header("Component")]
    public GameObject player;

    [Header("Time")]
    public Text timeText;
    public static float seconds;
    public static float minutes;

    public static bool isPaused = true;
    public static bool youWin;
    public GameObject pauseMenu;

    [Header("Coin")]
    public Text coinText;
    public static int coins;

    [Header("Spawns")]
    public Spawn[] spws;

    [Header("Buttons")]
    public GameObject buttonStart;
    public GameObject buttonYouWin;

    // Start is called before the first frame update
    void Start()
    {
        youWin = false;

        seconds = 0;
        minutes = 0;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Timing();
        coinText.text = coins.ToString("00");

        if(youWin)
        {
            buttonYouWin.SetActive(true);
            isPaused = true;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused){
                ResumeGame();
            }else{
                PauseGame();
            }
        }
    }

    void Timing()
    {
        if(!isPaused)
        {
            seconds += Time.deltaTime;
        
            if(seconds > 59f)
            {
                minutes++;
                seconds = 0;
            }
            //secondsText.text = seconds.ToString("00");
            //minutesText.text = minutes.ToString("00");
            timeText.text = minutes.ToString("00") + ':' + seconds.ToString("00");
        }
    }

    public static void TakeCoin()
    {
        coins++;
    }

    public void StartGame()
    {
        for(int i = 0; i < 7; i++)
        {
            spws[i].isSpw = true; 
        }
        buttonStart.SetActive(false);
        isPaused = false;
        player.GetComponent<Player>().onFire = true;
    }

    public void NewGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);         
        Time.timeScale = 1f;
        isPaused = false;
    }
}
