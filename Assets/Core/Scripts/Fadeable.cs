using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class Fadeable : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> Renderers;
    [SerializeField] private float TimeToAppear;
    [SerializeField] private float TimeToDisappear;
    private List<TweenBase> activeTweens = new List<TweenBase>();
    public bool IsFaded { get; private set; } = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearTweens()
    {
        foreach (var tween in activeTweens)
        {
            tween.Cancel();
        }
        
        activeTweens.Clear();
    }

    [Button]
    public void FadeIn()
    {
        IsFaded = false;
        ClearTweens();
        
        foreach (var renderer in Renderers)
        {
            var tween = Tween.Color(renderer, renderer.color.WithAlpha(1f), TimeToAppear, 0f, Tween.EaseLinear);
        }
    }

    [Button]
    public void FadeOut()
    {
        IsFaded = true;
        ClearTweens();
        
        foreach (var renderer in Renderers)
        {
            var tween = Tween.Color(renderer, renderer.color.WithAlpha(0f), TimeToDisappear, 0f, Tween.EaseLinear);
        }
    }
}
