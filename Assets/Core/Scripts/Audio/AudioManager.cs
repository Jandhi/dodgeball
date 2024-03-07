using System;
using System.Collections;
using System.Collections.Generic;
using Pixelplacement;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioPlayer Throw;

    void Start()
    {
        Initialize(this);
    }
}
