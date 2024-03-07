using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public bool canGrab;
    public Transform obj;
    public Transform grabbingPoint;
    public float attackRadius = 1;
    public LayerMask enemyLayer;
    public bool alreadyGrabbed;
    public Collider2D[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyGrabbed) return;

        if(collision.tag == "enemy")
        {
            canGrab = true;
            obj = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (alreadyGrabbed) return;

        if (collision.tag == "enemy")
        {
            canGrab = false;
            obj = null;
        }
    }

    public void Grab()
    {
        obj.SetParent(grabbingPoint);
        obj.GetComponent<Rigidbody2D>().isKinematic = true;
        obj.localPosition = Vector3.zero;
        obj.GetComponent<EnemyAI>().canAttack = false;
    }

    public void Throw(Vector3 direction, float speed)
    {
        obj.parent = null;
        obj.GetComponent<Rigidbody2D>().isKinematic = false;
        obj.GetComponent<Rigidbody2D>().AddForce(direction * speed);
        obj.GetComponent<EnemyAI>().CanAttack();
    }


    public void Attacking(float attackDamage)
    {
        enemies = Physics2D.OverlapCircleAll(grabbingPoint.position, attackRadius, enemyLayer);

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Health>().HealthUpdate(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(grabbingPoint.position, attackRadius);
    }
}
