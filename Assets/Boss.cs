using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float timeBetweenAttack;
    public float AttackDistance = 5;
    public float speed;
    public bool canAttack = true;
    public Transform player, sword;
    public ParticleSystem attack;
    private Rigidbody2D rb;
    public Transform healthBar;
    public Vector3 healthbarOffset;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (healthBar)
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
        

    }
}
