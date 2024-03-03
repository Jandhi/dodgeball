using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Throwable : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    public void Throw(Vector2 force, Vector3 playerPosition)
    {
        
        var ball = Instantiate(projectilePrefab, playerPosition, Quaternion.identity);
        ball.GetComponent<Rigidbody2D>().AddForce(force);
        Destroy(this);
    }
}
