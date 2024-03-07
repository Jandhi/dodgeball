using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioPlayer : MonoBehaviour
{
    public List<AudioClip> Clips;
    private List<AudioSource> _sources = new List<AudioSource>();
    public float Volume;
    public float MaxPanAmount;
    public float PitchVariance;

    void Awake()
    {
        foreach (var clip in Clips)
        {
            var source = this.AddComponent<AudioSource>();
            source.clip = clip;
            _sources.Add(source);
            
            Debug.Log($"adding source {source}");
        }
    }
    
    [Button]
    public void Play(float xPosition = 0)
    {
        if (_sources.Count == 0)
        {
            Debug.LogError($"No audio sources in {this}");
            return;
        }
        
        var index = Random.Range(0, _sources.Count);
        var source = _sources[index];
        
        source.volume = Volume;
        source.panStereo = CalculatePan(xPosition);
        source.pitch = Random.Range(1f - PitchVariance, 1f + PitchVariance);
        source.Play();
    }

    private float CalculatePan(float xPosition)
    {
        var percent = (xPosition) / 12f;
        return percent * MaxPanAmount;
    }
}