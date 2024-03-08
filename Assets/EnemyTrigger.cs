using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public float damagePower = 10;
    public bool forAll = false;
    public bool canCollide = false;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().HealthUpdate(damagePower);
            GameObject.Find("hit").GetComponent<AudioSource>().Play();

        }
        if(forAll)
        {
            if (collision.tag == "enemy")
            {
                collision.GetComponent<Health>().HealthUpdate(damagePower * 2);
                GameObject.Find("hit").GetComponent<AudioSource>().Play();

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().HealthUpdate(damagePower);
            GameObject.Find("hit").GetComponent<AudioSource>().Play();

        }
        if (forAll)
        {
            if (collision.gameObject.tag == "enemy")
            {
                collision.gameObject.GetComponent<Health>().HealthUpdate(damagePower * 2);
                GameObject.Find("hit").GetComponent<AudioSource>().Play();

            }
        }
    }
}
