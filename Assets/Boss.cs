using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Boss : MonoBehaviour
{
    public float timeBetweenAttack;
    public float AttackDistance = 5, swordDistance;
    
    public float speed;
    public bool canAttack = true, velnurable;
    public Transform player, sword;
    public ParticleSystem laserAttack, shield;
    public GameObject tnts;
    private Rigidbody2D rb;
    public Transform healthBar;
    public Vector3 healthbarOffset;
    public GameObject laserLine, tntLines;
    public bool isAttacking = false;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        velnurable = false;
        healthBar.parent = null;
        DisableShield();
    }

    void FixedUpdate()
    {
        if (healthBar)
            healthBar.position = transform.position + healthbarOffset;
        if (spriteRenderer)
            spriteRenderer.transform.rotation = Quaternion.identity;

        if (!canAttack) return;

        spriteRenderer.flipX = player.position.x - transform.position.x > 0;

        Vector3 dir = (player.position - transform.position).normalized;

        if (Vector3.Distance(transform.position, player.position) > AttackDistance)
            Chase(dir);
        Look(dir);

        if (!isAttacking)
        {
            timeBetweenAttack -= Time.deltaTime;
            if (timeBetweenAttack <= 0)
            {
                isAttacking = true;
                RandomAttack();
                timeBetweenAttack = Random.Range(3, 10);
            }
        }


    }

    async void AttackLaser()
    {
        laserLine.SetActive(true);
        laserAttack.Play();
        laserAttack.GetComponent<AudioSource>().Play();
        await Delay(8);
        laserLine.SetActive(false);
        isAttacking = false;
    }

    void AttackSword()
    {
        AttackDistance = 1.3f;
        speed = 0.8f;
        sword.DOScale(1, 0.5f);
        sword.DOLocalRotate(new Vector3(0, 0, 3600), 20).SetRelative(true).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            StopSword();
        });
    }
    public void StopSword()
    {
        AttackDistance = 5;
        sword.DOScale(0, 0.5f);
        speed = 0.1f;
        isAttacking = false;
    }

    public async void DisableShield()
    {
        shield.Stop();
        velnurable = true;
        canAttack = false;
        await Delay(10);
        shield.Play();
        velnurable = false;
        canAttack = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (velnurable) return;

        if (collision.gameObject.TryGetComponent<Health>(out Health h))
            h.HealthUpdate(1);
    }

    // Update is called once per frame
    

    void RandomAttack()
    {
        int val = Random.Range(0, 3);

        switch (val)
        {
            case 0: AttackLaser(); break;
            case 1: AttackSword(); break;
            case 2: ThrowTNT(); break;

            default: ThrowTNT(); break;
        }
    }

    void Chase(Vector3 direction)
    {
        if (player != null)
        {
            // Move towards the player using Rigidbody2D
            rb.velocity = direction * speed;

            // Rotate towards the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        }
    }

    void Look(Vector3 direction)
    {
        // Rotate towards the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    }

    async void ThrowTNT()
    {
        tntLines.SetActive(true);
        await Delay(1);
        isAttacking = false;
        GameObject obj = Instantiate(tnts, transform.position, tntLines.transform.rotation);
        obj.transform.DOScale(2, 1);
        tntLines.SetActive(false);
        await Delay(0.5f);
        Collider2D[] TNTs = obj.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D tnt in TNTs)
        {
            tnt.enabled = true;
        }
    }

    public static Task Delay(float seconds)
    {
        return Task.Delay((int)(seconds * 1000));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, AttackDistance);
    }
}
