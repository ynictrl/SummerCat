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

    [Header("Coin")]
    public Text coinText;
    public static int coins;

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

    public static void TakeCoin()
    {
        coins++;
    }
   
}
