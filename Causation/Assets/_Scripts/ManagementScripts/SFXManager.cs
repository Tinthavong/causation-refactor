using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    //Array of all sound effects we have for stuff
    public Sounds[] sfxSounds;

    public static SFXManager instance;

    private void Awake()
    {
        //This makes SFXManager a singleton so that it can play stuff always between scenes
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //Grabs effects in Sounds.cs and applies their settings to it
        foreach (Sounds sfx in sfxSounds)
        {
            sfx.source = gameObject.AddComponent<AudioSource>();
            sfx.source.clip = sfx.audioClip;

            sfx.source.volume = sfx.volume;

            sfx.source.loop = sfx.loop;
        }
    }

    //Plays audio, you need to put the code in at the desired location with its proper name put in the inspector
    //Example: BulletScript.cs, Line 39
    public void PlayAudio(string name)
    {
        Sounds sounds = System.Array.Find(sfxSounds, sfxSounds => sfxSounds.name == name);

        if(sounds == null)
        {
            return;
        }

        sounds.source.Play();
    }
}
