using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHoverDetection : MonoBehaviour
{

    private TextMeshProUGUI text;
    
    // Start is called before the first frame update
    
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        Debug.Log(text.text);
    }
    public void Print()
    {
        Debug.Log("Print");
    }
    void OnMouseOver()
    {
        Debug.Log("Mouse enter");
        // This is called when the mouse pointer enters the UI element
        if (text != null)
        {
            text.color = Color.red; // Change text color to red as an example
        }
    }

    void OnMouseExit()
    {
        // This is called when the mouse pointer exits the UI element
        if (text != null)
        {
            text.color = Color.black; // Reset text color to the original color
        }
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
