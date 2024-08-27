using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WireTargetScript : MonoBehaviour
{
    public bool connected = false;
    public GameObject connectedObject = null;
    public Color color;

    public bool CanConnect()
    {
        return !connected;
    }

    public void ConnectToWire(GameObject wireObject)
    {
        if (!connected)
        {
            connected = true;
            connectedObject = wireObject;
        }
    }

    public void DisconnectFromWire()
    {
        connected = false;
        connectedObject = null;
    }

    public void _Reset()
    {
        connected = false;
        connectedObject = null;
    }
}