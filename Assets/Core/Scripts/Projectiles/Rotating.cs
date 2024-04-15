using UnityEngine;
using UnityEngine.Serialization;

public class Rotating : MonoBehaviour
{
    [SerializeField] private float force;
    
    void Awake()
    {
        GetComponent<Projectile>().OnThrow += OnThrow;
    }

    void OnThrow(Rigidbody2D rb)
    {
        rb.AddTorque(force);
    }
}