using UnityEngine;

[CreateAssetMenu(fileName = "SpawnableSettings", menuName = "Dodgeball/SpawnableSettings")]
public class SpawnableSettings : ScriptableObject
{
    public float SpawnDuration;
    public float DespawnDuration;
    public AnimationCurve SpawnCurve;
    public AnimationCurve DespawnCurve;
}