using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BatteryIndicator : MonoBehaviour
{

    public TextMeshProUGUI batteryText;
    public GameObject[] greenBatteries;
    public GameObject[] yellowBatteries;
    public GameObject[] orangeBatteries;
    public GameObject[] redBatteries;
    public GameObject[] batteries;

    private Color yellow = new Color32(229, 231, 34, 255);
    private Color red = new Color32(231, 58, 34, 255);
    private Color orange = new Color32(211, 108, 82, 255);
    private Color green = new Color32(1, 255, 8, 255);
    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerData.batteriesLevel > 6) {
            batteries = greenBatteries;
            batteryText.color = green;
        }
        else if (PlayerData.batteriesLevel > 4) {
            foreach (GameObject battery in greenBatteries)
            {
                battery.SetActive(false);
            }
            batteries = yellowBatteries;
            batteryText.color = yellow;
        }
        else if (PlayerData.batteriesLevel > 2) {
            foreach (GameObject battery in yellowBatteries)
            {
                battery.SetActive(false);
            }
            batteries = orangeBatteries;
            batteryText.color = orange;
        }
        else {
            foreach (GameObject battery in orangeBatteries)
            {
                battery.SetActive(false);
            }
            batteries = redBatteries;
            batteryText.color = red;
        }

        for (int i=0; i < batteries.Length; i++)
        {
            if (i < PlayerData.batteriesLevel)
            {
                batteries[i].SetActive(true);
            }
            else
            {
                batteries[i].SetActive(false);
            }
        }
        batteryText.text = PlayerData.batteriesLevel.ToString() + "/10";
    }
}
