using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    bool catching;
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
        Debug.Log(_spriteR);
        bool emptyHands = _carrier.Throwable is null;
        if (emptyHands) 
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
		Debug.Log(collision.gameObject);
		if (catching)
		{
			// ball is caught and ownership goes to this player now
		}
    }
}
