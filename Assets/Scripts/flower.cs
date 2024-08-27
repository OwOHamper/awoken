using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flower : MonoBehaviour
{
    public GameObject flowerGood;
    public GameObject flowerBad;
    public bool watered;
    public int flowerIndex;
    private Dialogue dialogue;
    public bool isOutside;
    

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


    private void Awake() {
        watered = PlayerData.flowersWateredL[flowerIndex-1];
        if (watered) {
            flowerBad.SetActive(false);
            flowerGood.SetActive(true);
        }
        else {
            flowerBad.SetActive(true);
            flowerGood.SetActive(false);
        }

    }

    public void waterFlower() {
        if (GetDialogue().inDialogue || !PlayerData.canMove) {
            return;
        }

        if (isOutside)
        {
            FindObjectOfType<PlayerMovement>().GoToFlower(flowerIndex, this);
        } else
        {
            WaterAfterMovement();
        }

    }

    public void WaterAfterMovement()
    {
        flowerBad.SetActive(false);
        flowerGood.SetActive(true);
        watered = true;
        PlayerData.flowersWateredL[flowerIndex - 1] = true;
        GetDialogue().StartDialogue("flower");
        GameObject.Find("SceneScripts").GetComponent<Tooltip>().Hide();
    }
}
