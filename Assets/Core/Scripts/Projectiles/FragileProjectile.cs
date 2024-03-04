using UnityEngine;

public class FragileProjectile : BaseProjectile
{
    public GameObject OnDeathParticles;
    
    public override void OnProjectileCollision(Collision2D collision)
    {
        if (OnDeathParticles is not null) Instantiate(OnDeathParticles, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}