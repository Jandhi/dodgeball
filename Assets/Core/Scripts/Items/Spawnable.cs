using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using Sirenix.OdinInspector;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    public SpawnableSettings Settings;

    [Button]
    public void Spawn()
    {
        transform.localScale = Vector3.zero;
        Tween.LocalScale(transform, Vector3.one, Settings.SpawnDuration, 0f, Settings.SpawnCurve);
    }

    [Button]
    public void Despawn()
    {
        Tween.LocalScale(transform, Vector3.zero, Settings.DespawnDuration, 0f, Settings.DespawnCurve);
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(Settings.DespawnDuration);
        Destroy(this);
        yield return null;
    }
}
