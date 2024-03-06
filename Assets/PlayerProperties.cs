using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerProperties : MonoBehaviour
{
    public float endurance = 50;
    public float punchForce = 5;
    public Transform inner;
    public ParticleSystem attack, heavyAttack;
    public float speed = 5f; // Adjust this value to control player speed
    public bool special, alternate;
    public PlayerTrigger playerTrigger;
    private Rigidbody2D rb;
    private Vector3 mousePosition;
    bool throwing, grabbed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MousePosition();
        Movement();
        LookAtCursor();

        special = Input.GetKey(KeyCode.LeftShift);
        alternate = Input.GetKey(KeyCode.LeftControl);

        if (Input.GetMouseButtonDown(0))
        {
            if (special) HeavyAttack();
            else Attack();

        }

        if(Input.GetMouseButton(1))
        {
            Grab();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            Throw();
        }
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

        if(!throwing)
            rb.rotation = angle;
        
    }

    void Attack()
    {
        attack.Play();
        playerTrigger.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.3f).SetRelative(true).SetEase(Ease.InElastic);
       // Vector3 dir = (mousePosition - transform.position).normalized;
       // rb.AddForce(dir * punchForce);
    }

    void HeavyAttack()
    {
        heavyAttack.Play();
        Vector3 dir = (mousePosition - transform.position).normalized;
        rb.AddForce(dir * punchForce * 2);
    }

    void Grab()
    {
        if(playerTrigger.canGrab)
        {
            playerTrigger.Grab();
            grabbed = true;
        }
    }

    public void Throw()
    {
        if (!grabbed) return;
        throwing = true;
        inner.DOLocalRotate(new Vector3(0, 0, 360), 0.3f).SetRelative(true).SetEase(Ease.InCubic).OnComplete(() =>
        {
            Vector3 Dir = mousePosition - transform.position;
            playerTrigger.Throw(Dir, punchForce);
            inner.localRotation = Quaternion.identity;
            throwing = false;
        });
        
    }
}
