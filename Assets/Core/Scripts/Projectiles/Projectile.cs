using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject InHandsObject;
	public Weight Weight;
	public int Damage;
	public bool Catchable;
	
	Rigidbody2D rb;
	Collider2D col;
	GameObject owner;
	Collider2D[] ownerColliders;

	public delegate void ThrowHandler(Rigidbody2D rb);
	public event ThrowHandler OnThrow;

	public delegate void CollisionHandler(Collision2D collision);

	public event CollisionHandler OnCollision;

	private void OnDestroy()
	{
		GameManager.Instance.DespawnMe(gameObject);
	}

	public virtual void Throw(Vector2 force, GameObject _owner)
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

		OnThrow?.Invoke(rb);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// we've hit something... allow collisions with owner again
		foreach (var collider in ownerColliders)
		{
			Physics2D.IgnoreCollision(collider, col, false);
		}
		
		OnCollision?.Invoke(collision);
	}
}
