using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Throwable : MonoBehaviour
{
    public float SpawnDistance;
    [SerializeField] private GameObject projectilePrefab;
    public bool IsDone = false;
    public Weight Weight;

    public void Start()
    {
	    Weight = projectilePrefab.GetComponent<Projectile>().Weight;
    }

    public void Throw(Vector2 force, Vector3 position, GameObject owner)
    {
        Projectile ball = Instantiate(projectilePrefab, position, Quaternion.identity).GetComponent<Projectile>();
        ball.Throw(force, owner);

		GameManager.Instance.UpdateItemStatus(gameObject, ball.gameObject);

		AudioManager.Instance.Throw.Play(position.x);
		Destroy(gameObject);
	}
}
