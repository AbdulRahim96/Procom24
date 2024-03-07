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

    public void HealthUpdate(float val, bool effect = true)
    {
        if(isBoss)
        {
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
            onDieTrigger.Invoke();
        }
    }
}
