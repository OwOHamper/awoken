using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainLevelManager : MonoBehaviour
{
    public void ChangeVolume(float v)
    {
        Sound s = Array.Find(FindObjectOfType<AudioManager>().sounds, sound => sound.name == "Main theme");
        s.source.volume = v;
    }
}
