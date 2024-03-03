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
    private Movement _movement;
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
        _movement = GetComponent<Movement>();
    }

    public void ReleaseAim()
    {
        
        if (timeAimed > 0 && Carrier.Throwable is not null)
        {
            DoThrow();    
        }
        
        if(!arrowFade.IsFaded) arrowFade.FadeOut();
        
        timeAimed = 0;
    }

    void DoThrow()
    {
        var throwable = Carrier.Throwable;
        if(throwable is null) return;
        
        var percent = chargeCurve.Evaluate(timeAimed > timeToFullCharge ? 1f : timeAimed / timeToFullCharge);
        var force = Mathf.Lerp(minForce, maxForce, percent);
        var directionVector = -1 * _movement.LookVector.normalized;
        
        throwable.Throw(force * directionVector, transform.position + (directionVector * throwable.SpawnDistance).WithZ(0));

        if (throwable.IsDone)
        {
            Carrier.Throwable = null;
            Destroy(throwable.gameObject);
        }
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
