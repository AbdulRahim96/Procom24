using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectCoins : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            transform.SetParent(collision.transform);
            transform.DOLocalMove(Vector3.zero, 0.4f).OnComplete(() =>
            {
                GameLogic.instance.UpdateCoins(1);
                GameObject.Find("coins").GetComponent<AudioSource>().Play();
                Destroy(gameObject);
            });
        }
    }
}
