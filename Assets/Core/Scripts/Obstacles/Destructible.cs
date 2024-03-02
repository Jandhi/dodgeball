using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Pixelplacement;

public class Destructible : MonoBehaviour
{
	public int maxHitPoints;
	[ReadOnly] int hitpoints;
	[SerializeField] float shakeIntensity;
	[SerializeField] float shakeDuration;
	[SerializeField] GameObject dustPrefab;

	private void Start()
	{
		hitpoints = maxHitPoints;
	}
	public void Hit(int damage)
	{
		hitpoints -= damage;

		if (hitpoints > 0)
		{
			Tween.Shake(transform, transform.position, Vector3.one * shakeIntensity, shakeDuration, 0);
		}
		else
		{
			Destruct();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			Hit(1);
		}
	}

	public void Destruct()
	{
		Instantiate(dustPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}