using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public float min, max;
    public float spawnTime;
    public Transform[] positions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameLogic.isPaused) return;

        spawnTime -= Time.deltaTime;

        if (spawnTime <= 0)
        {
            int ran = Random.Range(0, positions.Length);
            Instantiate(enemy, positions[ran].position, Quaternion.identity);
            spawnTime = Random.Range(min, max);
        }
    }
}
