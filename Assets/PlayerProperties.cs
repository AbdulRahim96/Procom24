using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    public float endurance = 50;
    public float punchForce = 5;

    public float speed = 5f; // Adjust this value to control player speed

    private Rigidbody2D rb;
    public Vector3 mousePosition;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();
        LookAtCursor();

        if (Input.GetMouseButtonDown(0))
        {
            Attack();

        }
        MousePosition();
    }

    void Movement()
    {
        // Input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Movement vector
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Apply movement to rigidbody
        rb.velocity = movement * speed;

        // Clamp velocity to avoid diagonal movement being faster
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);
    }

    void MousePosition()
    {
        Vector3 m = Input.mousePosition;
        m.z = 10;
        mousePosition = Camera.main.ScreenToWorldPoint(m);
    }

    void LookAtCursor()
    {
        
        Vector3 lookDir = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        
    }

    void Attack()
    {
        print("Attack");
        Vector3 dir = (mousePosition - transform.position).normalized;
        rb.AddForce(dir * punchForce);
    }

    void HeavyAttack()
    {
        rb.AddForce(transform.forward * punchForce * 2, ForceMode2D.Impulse);
    }
}
