using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Core.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

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

    Vector2 aimInput, lookVector;
    Quaternion inputRotation;
	public float aimCutoff;

    IEnumerator aimSequence;

    private void Awake()
    {
        Carrier = GetComponent<Carrier>();
        arrow = transform.FindLogged("Arrow");
        arrowPointer = arrow.GetComponent<ThrowPointer>();
        arrowFade = arrow.GetComponent<Fadeable>();
        _movement = GetComponent<Movement>();
    }

    // this method is called only on frames where input values have changed.
	public void OnThrow(InputValue value)
	{
		aimInput = value.Get<Vector2>();

		if (aimInput.magnitude > aimCutoff)
        {
			lookVector = aimInput;

			// if we are not already aiming && have a throwable, start an aim
			if (aimSequence == null && Carrier.Throwable != null)
				Aim();
		}
	}

	public void Aim()
	{
		aimSequence = DoAim();
		StartCoroutine(aimSequence);
	}

	IEnumerator DoAim()
    {
		// slow the mover
		_movement.SetSlow(true);

        // while we continue to aim...
		while (aimInput.magnitude > aimCutoff)
        {
			inputRotation = Quaternion.Euler(Vector3.forward * (Mathf.Atan2(lookVector.y, lookVector.x) * Mathf.Rad2Deg));
			arrow.transform.rotation = inputRotation;

			AddTimeAimed(Time.deltaTime);

			yield return null;
        }

        // we are no longer aiming, fire!
        ReleaseAim();
        aimSequence = null;
		_movement.SetSlow(false);
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
        var directionVector = -1 * lookVector.normalized;
        
        throwable.Throw(force * directionVector, transform.position + (directionVector * throwable.SpawnDistance).WithZ(0), gameObject);

        // throwable is thrown. forget it
		Carrier.Throwable = null;
		//if (throwable.IsDone)
  //      {
  //          Carrier.Throwable = null;
  //          Destroy(throwable.gameObject);
  //      }
    }

    public void AddTimeAimed(float value)
    {
        if(arrowFade.IsFaded) arrowFade.FadeIn();
        timeAimed += value;

        var percent = chargeCurve.Evaluate(timeAimed > timeToFullCharge ? 1f : timeAimed / timeToFullCharge);
        arrowPointer.Stretch(percent);
    }
}
