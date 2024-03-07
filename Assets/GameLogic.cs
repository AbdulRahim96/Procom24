using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance;
    public float countDownTime = 200;
    public Text countDownTimeText;

    public static bool isPaused;
    public int coins;
    public Text cointext;
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        isPaused = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPaused)
        {
            countDownTime -= Time.deltaTime;
            countDownTimeText.text = countDownTime.ToString("0.00");
        }
    }

    public void UpdateCoins(int val)
    {
        coins += val;
        cointext.text = coins.ToString("0");
    }
}
