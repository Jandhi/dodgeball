using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
	public Vector2 GetMovementInput();
	public Vector2 GetAimInput();
}
