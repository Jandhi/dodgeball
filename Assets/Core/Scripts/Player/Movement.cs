using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using System;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
	[Header("Movement Metrics")]
	[PropertyTooltip("Max speed under normal circumstances.")]
	[SerializeField] float maxSpeed = 3f;
	[PropertyTooltip("Max speed while slowed.")]
	[SerializeField] float slowedSpeed = 1.5f;

	[Header("Dash Metrics")]
	[PropertyTooltip("The distance the player travels by dashing.")]
	[SerializeField] float dashLength;
	[PropertyTooltip("How long a dash takes to complete.")]
	[SerializeField] float dashDuration;
	[PropertyTooltip("The easing of the dash.")]
	[SerializeField] AnimationCurve dashCurve;

	Transform reticlePivot;
	float currentSpeed;
	Quaternion inputRotation;
	Rigidbody2D rb;
	Vector2 moveInput, moveVector, aimInput, lookVector;
	bool dashing;
	IEnumerator DashSequence;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponentInChildren<Rigidbody2D>();
		currentSpeed = maxSpeed;

		reticlePivot = transform.Find("ReticlePivot");
		if (reticlePivot == null)
			Debug.LogError("No reticle pivot object foumd");
    }

	public void OnMovement(InputValue value)
	{
		moveInput = value.Get<Vector2>();
	}

	public void OnDash()
	{
		Debug.Log("Dash pressed");
		// dashes are not interruptible
		if (DashSequence != null) return;

		dashing = true;
		DashSequence = Dash();
		StartCoroutine(DashSequence);
	}

	public void OnThrow(InputValue value)
	{
		aimInput = value.Get<Vector2>();
	}

	private void FixedUpdate()
	{
		if (!dashing) DoMovement();
		DoAim();
	}

	void DoAim()
	{
		// if there is no aim input...
		if (aimInput == Vector2.zero)
		{
			// if there is movement, use that (hide the reticle)
			if (moveInput != Vector2.zero)
			{
				lookVector.x = moveInput.y;
				lookVector.y = moveInput.x;
			}
			// otherwise maintain our aim as the last input. Do hide the reticle tho
			reticlePivot.gameObject.SetActive(false);
		}
		else
		{
			reticlePivot.gameObject.SetActive(true);
			lookVector = aimInput;
		}

		// rotate the reticle to point in the aim direction
		inputRotation = Quaternion.Euler(Vector3.forward * (Mathf.Atan2(lookVector.y, lookVector.x) * Mathf.Rad2Deg));
		reticlePivot.transform.rotation = inputRotation;
	}

	void DoMovement()
    {
		// calculate movement
		moveVector = moveInput * currentSpeed;

		// apply velocity
		rb.velocity = moveVector;
	}

	IEnumerator Dash()
	{
		// get movement vector
		Vector2 dashDir = moveInput.normalized * dashLength;

		// get start and end position of this dash
		Vector2 startPos = rb.position;
		Vector2 endPos = rb.position + dashDir;

		// evaluate dash motion
		Vector2 lastPos = startPos;
		float time = 0;
		while (time < dashDuration)
		{
			// find the next position to be at, apply speed needed to get there
			Vector2 nextPos = Vector2.Lerp(startPos, endPos, dashCurve.Evaluate(time / dashDuration));

			if (Time.timeScale != 0)
				rb.velocity = (nextPos - lastPos) / Time.deltaTime;
			else
				rb.velocity = Vector2.zero;

			time += Time.deltaTime;
			lastPos = nextPos;
			yield return null;
		}

		// dash is now completed.
		dashing = false;
		DashSequence = null;
	}
}
