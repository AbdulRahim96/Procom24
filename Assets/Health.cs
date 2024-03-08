using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxhealth;
    public Slider healthBar;
    public UnityEvent onDieTrigger;
    public GameObject hit;
    public bool isBoss = false;
    bool isDead;
    public void HealthUpdate(float val, bool effect = true)
    {
        if (isDead) return;

        if(isBoss)
        {
            if(healthBar.value < maxhealth/4)
            {
                GetComponent<Boss>().minTime = 2;
                GetComponent<Boss>().maxTime = 5;
                GetComponent<Boss>().swordSpeed = 1.5f;
                GetComponent<Boss>().swordDuration = 40f;
                GameLogic.Print("He's in Rage! Upgrade yourself");
            }
            if (!GetComponent<Boss>().velnurable) return;
        }


        if(effect)
        {
            GameObject obj = Instantiate(hit, transform.position, Quaternion.identity);
            Destroy(obj, 3);
        }
        healthBar.value -= val;
        if(healthBar.value <= 0)
        {
            healthBar.value = 0;
            // player Dead
            isDead = true;
            onDieTrigger.Invoke();
        }
        if (healthBar.value > maxhealth)
        {
            healthBar.value = maxhealth;
            
        }
    }
}
