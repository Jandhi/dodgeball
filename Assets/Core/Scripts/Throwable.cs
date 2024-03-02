using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Throwable : MonoBehaviour
{
    public float Force;
    private Rigidbody2D _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    void SendIt()
    {

        var rotation = Random.Range(0f, Mathf.PI * 2f);
        
        _rigidbody.AddForce(new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)) * Force);
    }
}
