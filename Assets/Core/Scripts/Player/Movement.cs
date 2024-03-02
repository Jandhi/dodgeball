using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Movement : MonoBehaviour
{
	[Header("Movement Metrics")]
	[PropertyTooltip("Max speed under normal circumstances.")]
	public float maxSpeed = 3f;
	[PropertyTooltip("Max speed while slowed.")]
	public float slowedSpeed = 1.5f;

	Transform reticlePivot;

	float currentSpeed;

	Quaternion inputRotation;
	Rigidbody2D rb;
	Vector2 moveInput, moveVector, aimInput, lookVector;
	IControllable playerController;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponentInChildren<Rigidbody2D>();
		currentSpeed = maxSpeed;
		playerController = GetComponent<IControllable>();

		if (playerController == null)
			Debug.LogError("No player controller found!");

		reticlePivot = transform.Find("ReticlePivot");
		if (reticlePivot == null)
			Debug.LogError("No reticle pivot object foumd");
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = playerController.GetMovementInput();
		aimInput = playerController.GetAimInput();
    }

	private void FixedUpdate()
	{
        DoMovement();
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
		inputRotation = Quaternion.Euler(Vector3.forward * (Mathf.Atan2(lookVector.x, lookVector.y) * Mathf.Rad2Deg));
		reticlePivot.transform.rotation = inputRotation;
	}

	void DoMovement()
    {
		// calculate movement
		moveVector = moveInput * currentSpeed;

		// apply velocity
		rb.velocity = moveVector;
	}
}
