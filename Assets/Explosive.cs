using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float explosionRadius = 5;
    public float damage = 30;
    public GameObject explosion;

    public void Explode()
    {
        GameObject obj = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(obj, 3);
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D player in players)
        {
            if (player.gameObject.GetComponent<Health>())
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                float dmg = damage / distance;
                if (player.gameObject.GetComponent<EnemyAI>())
                    player.gameObject.GetComponent<Health>().HealthUpdate(dmg * 5, false);
                else if (player.gameObject.GetComponent<Boss>())
                    player.gameObject.GetComponent<Health>().HealthUpdate(dmg / 5, false);
                else
                    player.gameObject.GetComponent<Health>().HealthUpdate(dmg, false);
                Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
                Vector2 dir = player.transform.position - transform.position;
                rb.AddForce(dir * dmg * 5);
            }
            if (player.gameObject.GetComponent<Boss>())
                player.gameObject.GetComponent<Boss>().DisableShield();
        }
        Destroy(gameObject);
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.3f);
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D player in players)
        {
            if (player.gameObject.GetComponent<Health>())
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                float dmg = damage / distance;
                if(player.gameObject.GetComponent<EnemyAI>())
                    player.gameObject.GetComponent<Health>().HealthUpdate(dmg * 5);
                else
                    player.gameObject.GetComponent<Health>().HealthUpdate(dmg);
                Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
                Vector2 dir = player.transform.position - transform.position;
                rb.AddForce(dir * dmg * 10);
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
            Explode();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
