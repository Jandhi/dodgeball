using System;
using UnityEngine;

public class FragileItem : MonoBehaviour
{
    public void Start()
    {
        GetComponent<Projectile>().OnCollision += OnProjectileCollision;
    }

    public GameObject OnDeathParticles;
    
    public void OnProjectileCollision(Collision2D collision)
    {
        if (OnDeathParticles is not null) Instantiate(OnDeathParticles, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}