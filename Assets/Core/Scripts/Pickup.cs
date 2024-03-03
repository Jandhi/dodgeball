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
        _triggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(_triggered) return;

        var carrier = other.GetComponentInParent<Carrier>();

        if (carrier is null) return;
        
        // Can't pick up if you have full hands
        if(carrier.Throwable is not null) return;
            
        _triggered = true;
            
        var inHands = Instantiate(InHands, other.transform.parent);
            
        _spawnable.Despawn();
        inHands.GetComponent<Spawnable>().Spawn();
        carrier.Throwable = inHands.GetComponent<Throwable>();
    }
}
