using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTooltip : MonoBehaviour
{
    private Tooltip tooltips;

    // Start is called before the first frame update
    void Start()
    {
        tooltips = GameObject.Find("SceneScripts").GetComponent<Tooltip>();
    }

    public void Show()
    {
        for (int i = 0; i < PlayerData.inventorySize; i++)
        {
            if (gameObject == Inventory.gridElements[i])
            {
                if (PlayerData.inventory[i] != "empty")
                {
                    tooltips.ShowItem(PlayerData.inventory[i]);
                }
            }
        }
    }

    public void Hide()
    {
        tooltips.Hide();
    }
}
