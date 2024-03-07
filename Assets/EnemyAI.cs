using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float timeBetweenAttack;
    public float AttackDistance = 5;
    public float speed;
    public bool alreadyAttacked;
    public bool canAttack = true;
    public Transform player;
    public bool onAir = false;
    private Rigidbody2D rb;
    public GameObject Coins;
    public Transform healthBar;
    public Vector3 healthbarOffset;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar.parent = null;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(healthBar)
            healthBar.position = transform.position + healthbarOffset;


        if (!canAttack) return;

        if (Vector3.Distance(transform.position, player.position) <= AttackDistance)
            attacking();
        else
            Chase();
    }

    void Chase()
    {
        print("chase");
        if (player != null)
        {
            // Calculate the direction towards the player
            Vector2 direction = (player.position - transform.position).normalized;

            // Move towards the player using Rigidbody2D
            rb.velocity = direction * speed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            rb.rotation = angle;
        }
    }

    public void attacking()
    {
        if (!alreadyAttacked)
        {
            Attack();
            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttack);
        }
    }

    void Attack()
    {

    }

    void resetAttack()
    {
        alreadyAttacked = false;
    }

    public void CanAttack()
    {
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        onAir = true;
        yield return new WaitForSeconds(1);
        onAir = false;
        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy" && onAir)
        {
            GetComponent<Health>().HealthUpdate(20);
            collision.gameObject.GetComponent<Health>().HealthUpdate(5);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-collision.transform.forward * 50000);
        }
    }

    public void Dead()
    {
        canAttack = false;
        Instantiate(Coins, transform.position, transform.rotation);
        healthBar.SetParent(transform);
        Destroy(gameObject, 0.5f);
    }
}
