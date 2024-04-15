using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedKill : MonoBehaviour
{
    [SerializeField] private float TimeToKill;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(TimeToKill);
        Destroy(gameObject);
        yield return null;
    }
}
