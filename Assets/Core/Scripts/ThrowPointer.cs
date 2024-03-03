using System;
using System.Collections;
using System.Collections.Generic;
using Core.Scripts.Utils;
using UnityEngine;

public class ThrowPointer : MonoBehaviour
{
    [SerializeField] private float minLength;
    [SerializeField] private float maxLength;
    [SerializeField] private float baseLineOffset;
    [SerializeField] private float baseHeadOffset;

    private Transform _square;
    private Transform _triangle;

    void Start()
    {
        _square = transform.FindLogged("Square");
        _triangle = transform.FindLogged("Triangle");

		// stow away the arrow onstart
		Stretch(0);
	}

    public void Stretch(float percent)
    {
        var size = Mathf.Lerp(minLength, maxLength, percent);
        var amountBigger = size - minLength;
        var addedOffset = -amountBigger / 2;

        var localScale = _square.localScale;
        localScale = new Vector3(localScale.x, size, localScale.z);
        _square.localScale = localScale;
        _square.localPosition = new Vector3(baseLineOffset + addedOffset, 0, 0);

        _triangle.localPosition = new Vector3(baseHeadOffset + 2 * addedOffset, 0, 0);
    }
}
