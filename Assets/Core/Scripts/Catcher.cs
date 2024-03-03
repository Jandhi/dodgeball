using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    [Header("Catch Settings")] 
    [SerializeField] public float catchWindow;

    Rigidbody2D rb;

    private void Start()
    {

        Carrier Carrier = GetComponent<Carrier>();
        Catchable Catchable = GetComponent<Catchable>();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    public void Catch()
    {
        // caught = true if caught within window frame
        // if (Carrier.Throwable is null && caught)
        // {
        //     DoCatch();
        // }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
