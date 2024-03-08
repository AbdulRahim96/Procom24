using DG.Tweening;
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
    public Transform player, sword;
    public ParticleSystem attack;
    public LayerMask layer;
    public bool onAir = false;
    private Rigidbody2D rb;
    public GameObject Coins;
    public Transform healthBar;
    public Vector3 healthbarOffset;
    public GameObject hitEffects;
    public SpriteRenderer spriteRenderer;
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
        if (spriteRenderer)
            spriteRenderer.transform.rotation = Quaternion.identity;

        if (!canAttack) return;

        spriteRenderer.flipX = player.position.x - transform.position.x > 0;

        if (Vector3.Distance(transform.position, player.position) <= AttackDistance)
            attacking();
        else
            Chase();
    }

    void Chase()
    {
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
        attack.Play();
        sword.DOLocalRotate(new Vector3(0, 0, -360), 0.3f).SetRelative(true).SetEase(Ease.InElastic);
        StartCoroutine(attackCollider());
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

    IEnumerator attackCollider()
    {
        yield return new WaitForSeconds(0.2f);
        sword.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        sword.GetComponent<Collider2D>().enabled = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            //if (!canAttack)
            //    FindObjectOfType<PlayerProperties>().Drop();
            if(onAir)
            {
                GetComponent<Health>().HealthUpdate(20);
                collision.gameObject.GetComponent<Health>().HealthUpdate(13, false);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(-collision.transform.forward * 50000);

                GameObject obj = Instantiate(hitEffects, transform.position, Quaternion.identity);
                Destroy(obj, 3);
            }
            
        }
    }

    public void Dead()
    {
        canAttack = false;
        Instantiate(Coins, transform.position, transform.rotation);
        healthBar.SetParent(transform);
        if (FindObjectOfType<PlayerTrigger>().obj == transform)
        {
            FindObjectOfType<PlayerTrigger>().obj = null;
            FindObjectOfType<PlayerTrigger>().canGrab = false;
            FindObjectOfType<PlayerTrigger>().alreadyGrabbed = false;
        }
        Destroy(gameObject, 0.5f);
        
    }
}
