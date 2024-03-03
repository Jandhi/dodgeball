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

    public void Throw(Vector2 force, Vector3 position)
    {
        
        var ball = Instantiate(projectilePrefab, position, Quaternion.identity);
        ball.GetComponent<Rigidbody2D>().AddForce(force);
        IsDone = true;
    }
}
