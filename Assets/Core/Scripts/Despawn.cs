using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    public float Lifetime;
    private Spawnable _spawnable;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnable = GetComponent<Spawnable>();
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(Lifetime);
        _spawnable.Despawn();
        yield return new WaitForSeconds(_spawnable.Settings.DespawnDuration);
        GameManager.Instance.DespawnMe(gameObject);
        yield return null;
    }
}
