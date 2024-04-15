using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    bool catching;
    private BaseProjectile caughtProjectile;
	private Carrier _carrier;
	private SpriteRenderer _spriteR;
    private Movement _movement;
    

    void Awake()
    {
		_spriteR = gameObject.GetComponentInChildren<SpriteRenderer>();
        _movement = GetComponent<Movement>();
        _carrier = GetComponent<Carrier>();
    }
    public void OnCatch()
    {
        bool holdingThrowable = _carrier.Throwable;
        if (!holdingThrowable)
        {
			StartCoroutine(Catch());
        }
    }
    IEnumerator Catch()
	{
		catching = true;
		_movement.SetSlow(true);
		_spriteR.color = Color.red;

		float time = 0;
		while (time < 0.25)
		{
			time += Time.deltaTime;
			yield return null;
		}
		_spriteR.color = Color.white;
		catching = false;
		_movement.SetSlow(false);
	}

	void OnCollisionEnter2D(Collision2D collision)
    {
        caughtProjectile = collision.gameObject.GetComponent<BaseProjectile>();
        Debug.Log(caughtProjectile);
		if (catching & caughtProjectile.Catchable)
		{
			OnSuccessfulCatch();
		}
    }

    public void OnSuccessfulCatch()
    {
        // Do something
        Debug.Log("YOU CAUGHT SOMETHINHG");
    }
}
