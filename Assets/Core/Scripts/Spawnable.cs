using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using Sirenix.OdinInspector;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public float SpawnDuration;
    public AnimationCurve Curve;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [Button]
    void Spawn()
    {
        transform.localScale = Vector3.zero;
        Tween.LocalScale(transform, Vector3.one, SpawnDuration, 0f, Curve);
    }
}
