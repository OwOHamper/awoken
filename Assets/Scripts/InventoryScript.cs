using Defective.JSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public static GameObject[] gridElements = new GameObject[PlayerData.inventorySize];
    public static Transform gridParent;
    public static GameObject gridElementPrefab = null;
    public static JSONObject itemsJson = null;
    private static bool firstPopulate = true;
    private static Inventory invInstance;
    private static Color color;
    private static string[] badScenes = new string[] {"MainMenu", "Endings"};


    public static int GetItemIndex(string id)
    {
        for (int i = 0; i < itemsJson["items"].list.Count; i++)
        {
            if (itemsJson["items"][i]["id"].stringValue == id)
            {
                return i;
            }
        }

        Debug.LogError("No item: " + id + "exists.");
        return 0;
    }


    private void Awake()
    {
        if (invInstance == null)
        {
            invInstance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        // string path = "Assets/Json/items.json";
        // string jsonString = File.ReadAllText(path);
        itemsJson = new JSONObject(JsonData.items);
        gridElementPrefab = Resources.Load<GameObject>("Prefabs/invItem");
        color = gridElementPrefab.GetComponent<UnityEngine.UI.Image>().color;
        // if (executed)
        // {
            

        //     // if (PlayerData.firstScript)
        //     // {
        //         // Debug.Log("test222");
        //         // Destroy(gameObject);
        //     // }
            
        // }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!badScenes.Contains(scene.name))
        {
            PopulateGrid();
        }
        else
        {
            firstPopulate = true;
        }
    }


    public static bool IsInInventory(string name)
    {
        for (int i = 0; i < PlayerData.inventorySize; i++)
        {
            if (PlayerData.inventory[i] == name)
            {
                return true;
            }
        }

        return false;
    }

    public static void AddItem(string name)
    {
        if (name == null)
        {
            Debug.LogError("item is null");
            return;
        }

        if (PlayerData.inventoryItemCount == PlayerData.inventorySize)
            return;

        for (int i = 0; i < PlayerData.inventorySize; i++)
        {
            if (PlayerData.inventory[i] == "empty")
            {
                PlayerData.inventory[i] = name;
                var element = gridElements[i].transform.Find("Icon").GetComponentInChildren<UnityEngine.UI.Image>();
                element.sprite = Resources.Load<Sprite>(itemsJson["items"][GetItemIndex(name)]["asset"].stringValue);
                element.color = Color.white;
                PlayerData.inventoryItemCount += 1;
                break;
            }
        }
    }


    public static void RemoveItem(string name)
    {
        if (name == null)
        {
            Debug.LogError("name is null");
            return;
        }

        bool called = false;
        for (int i = 0; i < PlayerData.inventorySize; i++)
        {
            if (PlayerData.inventory[i] == name)
            {
                PlayerData.inventory[i] = "empty";
                var element = gridElements[i].transform.Find("Icon").GetComponentInChildren<UnityEngine.UI.Image>();
                element.sprite = gridElementPrefab.GetComponent<Sprite>();
                element.color = new Color(0f, 0f, 0f, 0f);
                PlayerData.inventoryItemCount -= 1;
                called = !called;
                break;
            }
        }

        if (!called)
            Debug.Log("Couldn't find item: " + name + " in inventory.");
    }

    private void PopulateGrid()
    {
        gridParent = GameObject.Find("Canvas").transform.Find("Inventory").Find("InventoryGrid");
        for (int i = 0; i < PlayerData.inventorySize; i++)
        {
            if (firstPopulate)
            {
                gridElements[i] = Instantiate(gridElementPrefab, gridParent);
                PlayerData.inventory[i] = "empty";
            }
            else
            {
                gridElements[i] = Instantiate(gridElementPrefab, gridParent);
                if (PlayerData.inventory[i] != "empty")
                {
                    var element = gridElements[i].transform.Find("Icon").GetComponentInChildren<UnityEngine.UI.Image>();
                    element.sprite = Resources.Load<Sprite>(itemsJson["items"][GetItemIndex(PlayerData.inventory[i])]["asset"].stringValue);
                    element.color = Color.white;
                }
            }
        }

        firstPopulate = false;
    }
}
