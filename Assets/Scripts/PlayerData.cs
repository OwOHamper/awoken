using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int batteriesLevel;
    public static int inventorySize;
    public static int inventoryItemCount;
    public static string[] inventory;
    public static string mayorStatus;
    public static bool? minigameSolderStatus;
    public static bool? minigameMusicStatus;
    public static bool inMinigame;
    public static string hamperState;
    public static bool talkedWithOliver;
    public static bool talkedWithOliverAfterAbbz;
    public static bool repaired;
    public static bool generatorFixed;
    public static bool abbzFirstTime;
    public static string abbzState;
    public static bool atmosphereDestroyed;
    public static bool canMove;
    public static int endingIndex;
    public static string itemToPickUp; //used in clickhandler to keep track which item to pick up after dialogue ends
    public static bool firstScript;
    public static bool shopSparePart;
    public static bool shopCatFood;
    public static bool shopTent;
    public static bool hamperTranslator;
    public static bool hamperHarmonica;
    public static bool hamperSparePart;
    public static int flowersWatered;
    public static bool[] flowersWateredL;
    public static bool catFed;
    public static Vector3 lastPos; // To spawn where you were before you changed scene
    public static Vector3 cameraLastPos;
    public static bool respawn;
    public static int lastRoadIdx;
    public static int wastedBatteriesToTinky;
    public static bool loadingFirstTime;
    public static bool isMoving;
    public static float volumeMultiplier = 1;


    private static ClickHandler clickHandler;



    public static void Initialize()
    {
        batteriesLevel = 10;
        inventorySize = 10;
        inventoryItemCount = 0;
        inventory = new string[inventorySize];
        mayorStatus = "undiscovered";
        minigameSolderStatus = null;
        minigameMusicStatus = null;
        inMinigame = false;
        hamperState = "neutral";
        talkedWithOliver = false;
        talkedWithOliverAfterAbbz = false;
        repaired = false;
        generatorFixed = false;
        abbzFirstTime = true;
        atmosphereDestroyed = false;
        abbzState = "neutral";
        canMove = true; // when a popup window in on the screen
        endingIndex = -1;
        itemToPickUp = "";
        firstScript = true;
        shopSparePart = true;
        shopCatFood = true;
        shopTent = true;
        hamperTranslator = true;
        hamperHarmonica = true;
        hamperSparePart = true;
        flowersWatered = 0;
        flowersWateredL = new bool [] {false, false, false, false, false, false, false, false};
        catFed = false;
        respawn = false;
        lastRoadIdx = -1;
        wastedBatteriesToTinky = 0;
        loadingFirstTime = true;
        isMoving = false;
    }

    public static bool AllSparePartsInInv() {
        for (int i = 1; i <= 6; i++)
        {
            if (!Inventory.IsInInventory("spare-part-" + i))
            {
                return false;
            }
        }
        return true;
    }

    public static void IncrementBatteryLevel(int amount)
    {   
        if (amount == 0) {
            wastedBatteriesToTinky += 1;
        }

        batteriesLevel += amount;
        if (batteriesLevel == 0) {
            if (inMinigame) {
                return;
            }
            if (wastedBatteriesToTinky == 10) {
                GetClickHandler().LoadEnding(5);
            }
            else if (AllSparePartsInInv()) {
                GetClickHandler().LoadEnding(2);
                // Debug.Log("Ending 3");
            }
            else {
                GetClickHandler().LoadEnding(3);
                // Debug.Log("Ending 4");
            }
        }
    }

    public static void CheckAIEnding()
    {
        if ((abbzState == "angry") && (hamperState == "angry") && (atmosphereDestroyed == true) && (minigameMusicStatus == false)) {
            GetClickHandler().LoadEnding(7);
        }
    }


    public static void IncrementFlowersWatered()
    {
        flowersWatered += 1;
        if (flowersWatered == 8 && catFed) {
            GetClickHandler().LoadEnding(6);
        }
    }

    public static void FeedCat()
    {
        catFed = true;
        if (flowersWatered == 8 && catFed) {
            GetClickHandler().LoadEnding(6);
        }
    }

    private static void checkIfValid()
    {
        if (clickHandler == null)
        {
            // clickHandler = FindObject
            clickHandler = GameObject.Find("clickHandler").GetComponent<ClickHandler>();
        }
    }

    private static ClickHandler GetClickHandler()
    {
        checkIfValid();
        return clickHandler;
    }


}
