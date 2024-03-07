using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticleSystem : MonoBehaviour
{
    [Range(1, 5)] public float timeGap = 5;
    public float currenTime;
    public float damage = 5;
    protected ParticleSystem bulletParticle;
    List<ParticleCollisionEvent> colevents = new List<ParticleCollisionEvent>();
    // Start is called before the first frame update
    void Start()
    {
        bulletParticle = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponentInParent<Health>())
        {
            other.GetComponentInParent<Health>().HealthUpdate(damage);
        }
        

    }

    private void OnParticleTrigger()
    {
        
    }
}
