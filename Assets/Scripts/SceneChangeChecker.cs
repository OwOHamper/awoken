using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeChecker : MonoBehaviour
{
    private Dialogue dialogue;
    private bool executed = false;


    private void Awake()
    {
        if (!executed)
        {
            // DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
            executed = false;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log(scene.name);
        if (scene.name == "Skyscraper2") {
            if(PlayerData.mayorStatus == "discovered") {
                GetDialogue().StartDialogue("michaldo");
            }
        }

        if (scene.name == "Workshop") {
            GetDialogue().StartDialogue("oliver-enter");
        }

        if (scene.name == "Shopkeeper") {
            if (PlayerData.abbzState != "happy") {
                GetDialogue().StartDialogue("abbz");
            }
        }
        // if (scene.name == "")
    }

    void checkIfValid()
    {
        if (dialogue == null)
        {
            dialogue = GameObject.Find("Scripts").GetComponent<Dialogue>();
        }
    }
    
    private Dialogue GetDialogue()
    {
        checkIfValid();
        return dialogue;
        // return GameObject.Find("Scripts").GetComponent<Dialogue>();;
    }
}
