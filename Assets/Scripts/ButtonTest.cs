using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using TMPro;


public class ButtonTest : MonoBehaviour
{
    // public Button Button1, Button2;
    // public GameObject Button1;
    // Start is called before the first frame update
    void Start()
    {
        // Button1.onClick.AddListener(delegate {TaskWithParameters("Hello1"); });
        // Button2.onClick.AddListener(delegate {TaskWithParameters("Hello2"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPress(string message)
    {
        Debug.Log("Button Pressed" + message);
    }
}

    // void TaskWithParameters(string message)
    // {
    //     //Output this to console when the Button2 is clicked
    //     Debug.Log(message);
    // }



