using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public LevelLoader loader;
    public int sceneIdx = 9;

    private void Awake()
    {
        PlayerData.Initialize();
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Main menu");
    }

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Stop("Main menu");
        FindObjectOfType<AudioManager>().Play("Main theme");
        loader.LoadNextLevel(sceneIdx);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
