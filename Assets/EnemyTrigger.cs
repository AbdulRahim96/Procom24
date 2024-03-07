using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public float damagePower = 10;
    public bool forAll = false;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().HealthUpdate(damagePower);
        }
        if(forAll)
        {
            if (collision.tag == "enemy")
            {
                collision.GetComponent<Health>().HealthUpdate(damagePower * 2);
            }
        }
    }
}
