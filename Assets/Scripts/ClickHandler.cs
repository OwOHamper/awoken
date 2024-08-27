using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;
using Defective.JSON;
using UnityEngine.SceneManagement;

public class ClickHandler : MonoBehaviour
{
    private LevelLoader levelLoader;
    private Dialogue dialogue;
    private EndingLoader endingLoader;

    public void ChangeScene(int sceneId)
    {
        // var levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        // var dialogueBox = GameObject.Find("Dialogue box").GetComponent<Dialogue>();
        if (!GetDialogue().inDialogue) {
            if (SceneManager.GetActiveScene().buildIndex == 1) // If active scene is Main Level
            {
                FindObjectOfType<PlayerMovement>().GoToBuilding(sceneId);
                return;
            }
            getLevelLoader().LoadNextLevel(sceneId);
        }
        
    }

    public void ChangeToMayorScene()
    {
        int sceneId = 5;
        string mayorStatus = PlayerData.mayorStatus;
        if (mayorStatus == "discovered" || mayorStatus == "saved")
        {
            ChangeScene(sceneId);
        }
        else if (mayorStatus == "dead" || mayorStatus == "undiscovered") 
        {
            GetDialogue().StartDialogue("mayor-room-prevent-enter");
        }

    }

    public void switchToOliverScene()
    {
        int sceneId = 3;
        if (PlayerData.atmosphereDestroyed) {
            FindObjectOfType<PlayerMovement>().GoToBuilding(sceneId, false, "oliver-enter");
        }
        else
        {
            ChangeScene(sceneId);
        }
    }

    public void switchToMainFromShop()
    {
        int sceneId = 1;

        if (!PlayerData.abbzFirstTime)
        {
            PlayerData.talkedWithOliverAfterAbbz = true;
        }
        ChangeScene(sceneId);
    }

    public void switchToAbbz()
    {
        int sceneId = 2;

        if (PlayerData.abbzState == "neutral") {
             ChangeScene(sceneId);
        }
        else if (PlayerData.abbzState == "angry") {
            FindObjectOfType<PlayerMovement>().GoToBuilding(sceneId, false, "abbz");
        }
        else if (PlayerData.abbzState == "happy") {
            ChangeScene(sceneId);
        }
    }

    public void runDialog(string destinationDialog)
    {
        GetDialogue().StartDialogue(destinationDialog);
    }

    private Dialogue GetDialogue()
    {
        checkIfValid();
        return dialogue;
        // return GameObject.Find("Scripts").GetComponent<Dialogue>();;
    }

    private LevelLoader getLevelLoader()
    {
        checkIfValid();
        return levelLoader;
    }

    private EndingLoader GetEndingLoader()
    {
        checkIfValid();
        return endingLoader;
    }


    void checkIfValid()
    {
        if (dialogue == null)
        {
            dialogue = GameObject.Find("Scripts").GetComponent<Dialogue>();
        }

        if (levelLoader == null)
        {
            levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        }

        if (endingLoader == null)
        {
            endingLoader = GameObject.Find("Scripts").GetComponent<EndingLoader>();
        }
    }

    public void LoadEnding(int endingIndex)
    {
        GetEndingLoader().LoadEnding(endingIndex);
    }


    public void Test()
    {
        Debug.Log("grr");
    }

    public void PickUpFromHamper(string item)
    {
        if (GetDialogue().inDialogue) {
            return;
        }

        if (PlayerData.hamperState == "happy") {
            PickUpItemWithCost(item);
        }
        else
        {
            GetDialogue().StartDialogue("hamper-pickup");
        }
    }

    public void PickUpItemWithCost(string item)
    {
        if (GetDialogue().inDialogue) {
            return;
        }

        if (PlayerData.batteriesLevel > 0)
        {
            PlayerData.itemToPickUp = item;
            GetDialogue().StartDialogue("pickup-item");
        }
        else
        {
            GetDialogue().StartDialogue("pickup-item-no-battery");
        }
    }

    public void PickUp(string item)
    {

        if (item == null)
        {
            Debug.Log("item: " + item + " is null");
            return;
        }

        if (PlayerData.inventoryItemCount == PlayerData.inventorySize)
        {
            Debug.Log("couldn't pickup item: " + item + ". Inventory is full!");
            return;
        }

        
        // var _item = GameObject.Find(item);
        // if (_item == null)
        // {
        //     Debug.Log(item + " is null");
        // }
        // Destroy(_item);

        // Debug.Log("picked up item: " + item);
        if (item == "spare-part-1") {
            PlayerData.shopSparePart = false;
        }
        else if (item == "cat-food") {
            PlayerData.shopCatFood = false;
        }
        else if (item == "tent") {
            PlayerData.shopTent = false;
        }
        else if (item == "translator") {
            PlayerData.hamperTranslator = false;
        }
        else if (item == "harmonica") {
            PlayerData.hamperHarmonica = false;
        }
        else if (item == "spare-part-3") {
            PlayerData.hamperSparePart = false;
        }

        Inventory.AddItem(item);
    }
}
