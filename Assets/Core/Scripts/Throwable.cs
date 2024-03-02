using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Throwable : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    void Throw(Vector2 force, Vector3 playerPosition)
    {
        Instantiate(projectilePrefab, playerPosition, Quaternion.identity);
    }
}
