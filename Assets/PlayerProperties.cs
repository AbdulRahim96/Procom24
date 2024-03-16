using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerProperties : MonoBehaviour
{
    public float endurance = 50;
    public float enduranceRate = 1;
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
    bool attacking;
    private bool pressing;

    [Header("Mobile")]
    public bool isMobile;
    public Joystick movestick, aim;
    public float offsetAngle;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool isPressed()
    {
        return (Mathf.Sqrt(aim.Horizontal) + Mathf.Sqrt(aim.Vertical) != 0);
    }

    void Update()
    {
        if (GameLogic.isPaused) return;

        MousePosition();
        Movement();
        LookAtCursor();
        Endurance();
        special = Input.GetKey(KeyCode.LeftShift);
        alternate = Input.GetKey(KeyCode.LeftControl);

        /*if (Input.GetMouseButtonDown(0))
        {
            if (special) HeavyAttack();
            else Attack();

        }*/

        if(Input.GetMouseButton(1))
        {
            Grab();
            if(playerTrigger.obj == null)
            {
                throwing = false;
                grabbed = false;
            }
        }
        else if(Input.GetMouseButtonUp(1))
        {
            if (playerTrigger.obj == null)
            {
                throwing = false;
                grabbed = false;
            }
            else
                Throw();
        }

        
    }

    void Movement()
    {
        // Input for movement
        float moveHorizontal;
        float moveVertical;
        if (isMobile)
        {
            moveHorizontal = movestick.Horizontal;
            moveVertical = movestick.Vertical;
        }
        else
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }




        pressing = moveHorizontal != 0;
        // Movement vector
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Apply movement to rigidbody
        rb.velocity = movement * speed;

        // Clamp velocity to avoid diagonal movement being faster
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);

        if (spriteRenderer)
            spriteRenderer.transform.rotation = Quaternion.identity;

            spriteRenderer.flipX = mousePosition.x - transform.position.x < 0;
    }

    void MousePosition()
    {
        Vector3 m = Input.mousePosition;
        m.z = 10;
        mousePosition = Camera.main.ScreenToWorldPoint(m);
    }

    void LookAtCursor()
    {
        Vector3 lookDir;
        if (isMobile)
            lookDir = new Vector3(aim.Horizontal,aim.Vertical,0);
        else
            lookDir = mousePosition - transform.position;
        float angle = (Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg) + offsetAngle;

        if(isPressed())
            rb.rotation = angle;
        
    }

    void Endurance()
    {
        if(grabbed)
        {
            endurance -= Time.deltaTime;
        }
        else
            endurance += enduranceRate * Time.deltaTime;
        enduranceSlider.value = endurance;

        if (endurance < 0)
            endurance = 0;
        if (endurance > 100)
            endurance = 100;
    }

    void UpdateEndurance(float val)
    {
        endurance += val;
        if (endurance <= 0) endurance = 0;
        if (endurance >= 100) endurance = 100;
    }

    public async void Attack()
    {
        if (attacking) return;

        attack.Play();
        playerTrigger.transform.DOLocalRotate(new Vector3(0, 0, -360), 0.3f).SetRelative(true).SetEase(Ease.InElastic);
        StartCoroutine(delay());
        GameObject.Find("light attack").GetComponent<AudioSource>().Play();
        attacking = true;
        await Boss.Delay(0.3f);
        attacking = false;
    }

    async void HeavyAttack()
    {
        if (attacking) return;
        if (endurance < 10) return;
        //Camera.main.DOShakePosition(0.3f, 1);
        UpdateEndurance(-10);
        heavyAttack.Play();
        Vector3 dir = (mousePosition - transform.position).normalized;
        rb.AddForce(dir * punchForce * 10);
        playerTrigger.transform.DOLocalRotate(new Vector3(0, 0, -360), 0.3f).SetRelative(true).SetEase(Ease.InElastic);
        StartCoroutine(delay(2));
        GameObject.Find("heavy attack").GetComponent<AudioSource>().Play();
        attacking = true;
        await Boss.Delay(0.6f);
        attacking = false;
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
            Vector3 Dir;
            if(isMobile)
                Dir = new Vector3(aim.Horizontal, aim.Vertical, 0);
            else
                Dir = mousePosition - transform.position;
            playerTrigger.Throw(Dir, punchForce);
            inner.localRotation = Quaternion.identity;
            throwing = false;
            endurance -= 20;
        });
        GameObject.Find("throw").GetComponent<AudioSource>().Play();

    }

    public void Drop()
    {
        throwing = false;
        grabbed = false;
    }

    public void Dead()
    {
        Instantiate(GameLogic.instance.gameoverMenu);
        Destroy(gameObject);
    }

    IEnumerator delay(float pow = 1)
    {
        yield return new WaitForSeconds(0.15f);
        playerTrigger.Attacking(power * pow);
        if(pow == 1)
            CameraShake.instance.impulseShake(0.3f, 0.3f);
        else
            CameraShake.instance.impulseShake(1, 0.3f);
    }

    void FakeDelay(float pow = 1)
    {
        GameObject dot = GameObject.Find(".");
        dot.transform.DOMoveX(1, 0.15f).OnComplete(() =>
        {
            playerTrigger.Attacking(power * pow);
            if (pow == 1)
                CameraShake.instance.impulseShake(0.3f, 0.3f);
            else
                CameraShake.instance.impulseShake(1, 0.3f);
        });
    }
}
