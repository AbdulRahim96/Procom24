using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance;
    public float countDownTime = 200;
    public Text countDownTimeText;
    public GameObject boss;
    public static bool isPaused;
    public int coins;
    public Text cointext;
    bool bossArrived;
    public GameObject fireBorders;
    public GameObject msgBox, gameoverMenu;
    public GameObject[] spawns;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        isPaused = false;
        bossArrived = false;
        cointext.text = coins.ToString("0");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPaused)
        {
            countDownTime -= Time.deltaTime;
            countDownTimeText.text = countDownTime.ToString("0.00");
            if(countDownTime <= 0)
            {
                countDownTimeText.text = "Boss Battle";
                if(!bossArrived)
                {
                    bossArrived = true;
                    BossBattle();
                }
            }
        }
    }

    public void UpdateCoins(int val)
    {
        coins += val;
        cointext.text = coins.ToString("0");
    }

    public async void BossBattle()
    {
        EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].Dead();
        }
        for (int i = 0; i < spawns.Length; i++)
        {
            spawns[i].SetActive(false);
        }


        SpriteRenderer background = GameObject.Find("background").GetComponent<SpriteRenderer>();
        background.DOColor(Color.blue, 3);
        fireBorders.SetActive(true);
        await Boss.Delay(3);
        Instantiate(boss);
        Print("Boss Battle");

        for (int i = 0; i < spawns.Length; i++)
        {
            spawns[i].SetActive(true);
        }

    }


    public static void Print(string mes)
    {
        if(!PlayerPrefs.HasKey(mes))
        {
            PlayerPrefs.SetInt(mes, 0);
            GameObject obj = Instantiate(instance.msgBox);
            obj.GetComponentInChildren<Text>().text = mes;
            Destroy(obj, 5);
        }
    }
}
