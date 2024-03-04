using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
	public GameObject InHandsObject;
	public Weight Weight;
	public int Damage;
	public bool Catchable;
	
	Rigidbody2D rb;
	Collider2D col;
	GameObject owner;
	Collider2D[] ownerColliders;

	public void Throw(Vector2 force, GameObject _owner)
	{
		rb = GetComponent<Rigidbody2D>();
		col = GetComponentInChildren<Collider2D>();
		owner = _owner;

		// protect projectile from collisions with its owner
		ownerColliders = owner.GetComponentsInChildren<Collider2D>();
		foreach (var collider in ownerColliders)
		{
			Physics2D.IgnoreCollision(collider, col);
		}
		rb.AddForce(force);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// we've hit something... allow collisions with owner again
		foreach (var collider in ownerColliders)
		{
			Physics2D.IgnoreCollision(collider, col, false);
		}
		
		OnProjectileCollision(collision);
	}

	public virtual void OnProjectileCollision(Collision2D collision) {}
}
