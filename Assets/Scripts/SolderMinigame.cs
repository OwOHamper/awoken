using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SolderMinigame : MonoBehaviour
{
    public GameObject solderMinigame;
    public static RectTransform currentWire = null;
    public static RectTransform currentTarget = null;
    private static int amount = 4;
    public static GameObject[] targets = new GameObject[amount];
    public static GameObject[] wires = new GameObject[amount];
    private Color[] availableColors;


    void Start()
    {
        Init();
    }

    private void Init()
    {
        availableColors = new Color[] { Color.blue, Color.red, Color.yellow, Color.green };
        ShuffleArray(availableColors);
        for (int i = 0; i < amount; i++)
        {
            targets[i] = transform.Find("target" + i).gameObject;
            targets[i].GetComponent<UnityEngine.UI.Image>().color = availableColors[i];
            targets[i].GetComponent<WireTargetScript>().color = availableColors[i];
        }

        ShuffleArray(availableColors);
        for (int i = 0; i < amount; i++)
        {
            wires[i] = transform.Find("wire" + i).gameObject;
            wires[i].GetComponent<UnityEngine.UI.Image>().color = availableColors[i];
            wires[i].GetComponent<WireScript>().color = availableColors[i];
        }
    }

    public void FailMinigame()
    {
        // Debug.Log("fail");
        PlayerData.minigameSolderStatus = false;
        PlayerData.inMinigame = false;
        solderMinigame.SetActive(false);
    }

    public void PassMinigame()
    {
        // Debug.Log("pass");
        PlayerData.minigameSolderStatus = true;
        PlayerData.inMinigame = false;
        solderMinigame.SetActive(false);
    }

    private void ShuffleArray(Color[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int randomIndex = Random.Range(i, n);

            Color temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    public void OnReset()
    {
        for (int i = 0; i < amount; i++)
        {
            wires[i].GetComponent<WireScript>().Revert();
        }
        Init();
    }

    public void OnDone()
    {
        for (int i = 0; i < amount; i++)
        {
            var target = targets[i].GetComponent<WireTargetScript>();
            if (!target.connected)
            {
                Debug.Log("Not all wires are connected!");
                return;
            }

            var wire = target.connectedObject.GetComponent<WireScript>();
            if (target.color != wire.color)
            {
                Debug.Log("Colors are incorrect!");
                FailMinigame();
                return;
            }
        }

        Debug.Log("Passed!");
        PassMinigame();
    }
}
