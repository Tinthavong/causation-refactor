using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Sounds is something that is changed in the SFXManager game object in the unity hierarchy
[Serializable]
public class Sounds
{
    public string name;

    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
