using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_PlayerController : MonoBehaviour, IControllable
{
	public Vector2 GetMovementInput()
	{
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	public Vector2 GetAimInput()
	{
		return new Vector2(Input.GetAxis("Aim_Horz"), Input.GetAxis("Aim_Vert"));
	}
}
