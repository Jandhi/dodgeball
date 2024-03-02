using System;
using System.Collections;
using System.Collections.Generic;
using Core.Scripts.Utils;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [Header("Throw Settings")] 
    [SerializeField] float minForce;
    [SerializeField] float maxForce;
    [SerializeField] float timeToFullCharge;
    [SerializeField] private AnimationCurve chargeCurve;

    private Carrier Carrier;
    Transform arrow;
    private ThrowPointer arrowPointer;
    private Fadeable arrowFade;
    float timeAimed = 0;

    private void Start()
    {
        Carrier = GetComponent<Carrier>();
        arrow = transform.FindLogged("Arrow");
        arrowPointer = arrow.GetComponent<ThrowPointer>();
        arrowFade = arrow.GetComponent<Fadeable>();
    }

    public void ReleaseAim()
    {
        timeAimed = 0;
        
        if (timeAimed > 0 && Carrier.Throwable is not null)
        {
            DoThrow();    
        }
        
        if(!arrowFade.IsFaded) arrowFade.FadeOut();
    }

    void DoThrow()
    {
        
    }

    public void AddTimeAimed(float value)
    {
        if(arrowFade.IsFaded) arrowFade.FadeIn();
        timeAimed += value;

        var percent = chargeCurve.Evaluate(timeAimed > timeToFullCharge ? 1f : timeAimed / timeToFullCharge);
        arrowPointer.Stretch(percent);
    }

    public void SetRotation(Quaternion inputRotation)
    {
        arrow.transform.rotation = inputRotation;
    }
}
