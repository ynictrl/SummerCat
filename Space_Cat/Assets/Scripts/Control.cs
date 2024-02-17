using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{

    [Header("Time")]
    public Text timeText;
    public static float seconds;
    public static float minutes;

    public static bool isPaused = true;

    [Header("Coin")]
    public Text coinText;
    public static int coins;

    [Header("Spawns")]
    public Spawn[] spws;

    [Header("Buttons")]
    public GameObject buttonStart;

    // Start is called before the first frame update
    void Start()
    {
        seconds = 0;
        minutes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Timing();
        coinText.text = coins.ToString("00");
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
    }
   
}
