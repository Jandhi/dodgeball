using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject InHands;
    private Spawnable _spawnable;
    private bool _triggered = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnable = GetComponent<Spawnable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_triggered) return;

        var carrier = other.GetComponentInParent<Carrier>();
        if (carrier is not null)
        {
            _triggered = true;
            
            var inHands = Instantiate(InHands, other.transform.parent);
            
            _spawnable.Despawn();
            inHands.GetComponent<Spawnable>().Spawn();
            carrier.Throwable = inHands.GetComponent<Throwable>();
        }
    }
}
