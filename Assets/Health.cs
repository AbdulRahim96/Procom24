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
    


    public void HealthUpdate(float val)
    {
        healthBar.value -= val;
        if(healthBar.value <= 0)
        {
            healthBar.value = 0;
            // player Dead
            onDieTrigger.Invoke();
        }
    }
}
