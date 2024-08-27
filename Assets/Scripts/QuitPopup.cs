using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitPopup : MonoBehaviour
{

    public Animator transition;
    private AudioManager audioManager;
    public Slider slider;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        slider.value = PlayerData.volumeMultiplier;
    }

    private void Update()
    {
        if (PlayerData.canMove && Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerData.canMove = false;
            transition.SetTrigger("Popup");
        }
    }

    public void Yes()
    {
        PlayerData.canMove = true;
        FindObjectOfType<AudioManager>().Stop("Main theme");
        FindObjectOfType<LevelLoader>().LoadNextLevel(0);
    }

    public void No()
    {
        transition.SetTrigger("ClosePopup");
        PlayerData.canMove = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeVolume(float v)
    {
        foreach (Sound s in audioManager.sounds)
        {
            s.source.volume = s.volume * v;
            PlayerData.volumeMultiplier = v;
        }
    }
}
