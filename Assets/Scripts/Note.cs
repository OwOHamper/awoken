using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public string sound;
    private MusicMinigame gameScript;
    public float enlargedScale = 1.3f;
    private bool playing = false;
    private int dontStop = 0;

    public void PlayNote()
    {
        if (playing)
        {
            dontStop++;
            StopPlaying(true);
        }
        gameScript.particleSystem.transform.position = transform.position;
        // var main = gameScript.particleSystem.GetComponent<ParticleSystem>().main;
        // main.startColor = GetComponent<UnityEngine.UI.Image>().color;
        gameScript.particleSystem.SetActive(true);
        FindObjectOfType<AudioManager>().Play(sound);
        playing = true;
        transform.localScale = new Vector3(enlargedScale, enlargedScale, transform.localScale.z);
    }

    public void StopPlaying(bool calledFromNote=false)
    {
        
        if (!calledFromNote && dontStop > 0)
        {
            dontStop--;
            return;
        }
        gameScript.particleSystem.SetActive(false);
        FindObjectOfType<AudioManager>().Stop(sound);
        playing = false;
        transform.localScale = new Vector3(1f, 1f, transform.localScale.z);
    }

    public void Awake()
    {
        gameScript = FindObjectOfType<MusicMinigame>();
    }

}
