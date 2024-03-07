using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerProperties : MonoBehaviour
{
    public float endurance = 50;
    public float punchForce = 5;
    public float power = 10;
    public Transform inner;
    public Slider enduranceSlider;
    public ParticleSystem attack, heavyAttack;
    public float speed = 5f; 
    public bool special, alternate;
    public PlayerTrigger playerTrigger;
    private Rigidbody2D rb;
    private Vector3 mousePosition;
    public bool throwing, grabbed;
    public SpriteRenderer spriteRenderer;

    private bool pressing;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MousePosition();
        Movement();
        LookAtCursor();
        Endurance();
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

        pressing = moveHorizontal != 0;
        // Movement vector
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Apply movement to rigidbody
        rb.velocity = movement * speed;

        // Clamp velocity to avoid diagonal movement being faster
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);

        if (spriteRenderer)
            spriteRenderer.transform.rotation = Quaternion.identity;

        if(pressing)
            spriteRenderer.flipX = moveHorizontal > 0;
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

    void Endurance()
    {
        if(grabbed)
        {
            endurance -= Time.deltaTime;
        }
        else
            endurance += Time.deltaTime;
        enduranceSlider.value = endurance;
    }

    void UpdateEndurance(float val)
    {
        endurance += val;
        if (endurance <= 0) endurance = 0;
        if (endurance >= 100) endurance = 100;
    }

    void Attack()
    {
        attack.Play();
        playerTrigger.transform.DOLocalRotate(new Vector3(0, 0, -360), 0.3f).SetRelative(true).SetEase(Ease.InElastic);
        StartCoroutine(delay());
        GameObject.Find("light attack").GetComponent<AudioSource>().Play();
       
    }

    void HeavyAttack()
    {
        if (endurance < 10) return;
        //Camera.main.DOShakePosition(0.3f, 1);
        UpdateEndurance(-10);
        heavyAttack.Play();
        Vector3 dir = (mousePosition - transform.position).normalized;
        rb.AddForce(dir * punchForce * 10);
        playerTrigger.transform.DOLocalRotate(new Vector3(0, 0, -360), 0.3f).SetRelative(true).SetEase(Ease.InElastic);
        StartCoroutine(delay(2));
        GameObject.Find("heavy attack").GetComponent<AudioSource>().Play();
    }

    void Grab()
    {
        if (grabbed) return;
        if (throwing) return;

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
        grabbed = false;
        inner.DOLocalRotate(new Vector3(0, 0, 360), 0.3f).SetRelative(true).SetEase(Ease.InCubic).OnComplete(() =>
        {
            Vector3 Dir = mousePosition - transform.position;
            playerTrigger.Throw(Dir, punchForce);
            inner.localRotation = Quaternion.identity;
            throwing = false;
        });
        GameObject.Find("throw").GetComponent<AudioSource>().Play();

    }

    public void Drop()
    {
        throwing = false;
        grabbed = false;
    }

    IEnumerator delay(float pow = 1)
    {
        yield return new WaitForSeconds(0.15f);
        playerTrigger.Attacking(power * pow);
    }
}
