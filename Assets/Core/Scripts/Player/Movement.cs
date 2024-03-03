using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using System;
using Core.Scripts.Utils;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
	[Header("Visuals")]
	[SerializeField] GameObject DashDustPrefab;
	ParticleSystem afterImage;

	float currentSpeed;
	Rigidbody2D rb;
	Vector2 moveInput, lastMoveInput, moveVector;
	IEnumerator DashSequence;

    // Start is called before the first frame update
    void Awake()
    {
		rb = GetComponentInChildren<Rigidbody2D>();
		currentSpeed = maxSpeed;
		afterImage = GetComponentInChildren<ParticleSystem>();

		lastMoveInput = Vector2.right;
    }

	public void OnMovement(InputValue value)
	{
		moveInput = value.Get<Vector2>();
		// we save the last known movement for dashes
		lastMoveInput = moveInput != Vector2.zero ? moveInput : lastMoveInput;
	}

	public void OnDash()
	{
		// dashes are not interruptible with more dashes.
		if (DashSequence != null) return;

		Instantiate(DashDustPrefab, transform.position, Quaternion.identity);
		afterImage.Play();
		DashSequence = Dash();
		StartCoroutine(DashSequence);
	}

	private void FixedUpdate()
	{
		// if there is no ongoing dash, move
		if (DashSequence == null) DoMovement();
	}

	public void SetSlow(bool set)
	{
		currentSpeed = set ? slowedSpeed : maxSpeed;
	}

	void DoMovement()
    {
		// calculate movement
		moveVector = (Vector2.ClampMagnitude(moveInput, 1)) * currentSpeed;
		
		// apply velocity
		rb.velocity = moveVector;
	}

	IEnumerator Dash()
	{
		// get movement vector from last movement
		Vector2 dashDir = lastMoveInput.normalized * dashLength;

		// get start and end position of this dash
		Vector2 startPos = rb.position;
		Vector2 endPos = startPos + dashDir;

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
		DashSequence = null;
	}
}
