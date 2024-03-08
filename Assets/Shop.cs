using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public PlayerProperties playerProperties;
    public Cost endurance, speed, power;

    public void buyEndurance()
    {
        if(endurance.Buy())
        {
            playerProperties.enduranceRate *= 2;
        }
    }

    public void buyspeed()
    {
        if (speed.Buy())
        {
            playerProperties.speed *= 1.5f;
        }
    }

    public void Play(bool flag)
    {
        GameLogic.isPaused = !flag;
        if (flag)
            Time.timeScale = 1;
        else
            Time.timeScale = 0;
    }

    public void buyPower()
    {
        if (power.Buy())
        {
            playerProperties.power *= 1.5f;
        }
    }

    [System.Serializable]
    public class Cost
    {
        public float[] amount;
        public int index;
        public Button button;
        public bool Buy()
        {
            if (GameLogic.instance.coins >= amount[index])
            {
                GameLogic.instance.UpdateCoins(-(int)amount[index]);
                index++;
                if (index == amount.Length)
                    button.interactable = false;
                return true;
            }
            else
                return false;
        }
    }
}
