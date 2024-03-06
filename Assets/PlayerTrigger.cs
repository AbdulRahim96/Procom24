using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public bool canGrab;
    public Transform obj;
    public Transform grabbingPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "enemy")
        {
            canGrab = true;
            obj = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
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
    }

    public void Throw(Vector3 direction, float speed)
    {
        obj.parent = null;
        obj.GetComponent<Rigidbody2D>().isKinematic = false;
        obj.GetComponent<Rigidbody2D>().AddForce(direction * speed);
    }
}
