using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePrefab : MonoBehaviour
{
    private Dialogue dialogue;
    

    public void TextHover(int n)
    {
        GetDialogue().TextHover(n);
    }

    public void TextHoverExit(int n)
    {
        GetDialogue().TextHoverExit(n);
    }

    public void TextClick(int n)
    {
        GetDialogue().TextClick(n);
    }

    public void StartDialogue(string d)
    {
        GetDialogue().StartDialogue(d);
    }

    public void AddItemToInv(string item)
    {
        GetDialogue().AddItemToInv(item);
    }


    void checkIfValid()
    {
        if (dialogue == null)
        {
            dialogue = GameObject.Find("Scripts").GetComponent<Dialogue>();
        }
    }
    // void Start()
    // {
    //     dialogue = GameObject.Find("Scripts").GetComponent<Dialogue>();
    // }
    
    private Dialogue GetDialogue()
    {
        checkIfValid();
        return dialogue;
        // return GameObject.Find("Scripts").GetComponent<Dialogue>();;
    }
    // private void Awake()
    // {
    //     dialogue = GameObject.Find("Scripts").GetComponent<Dialogue>();
    // }
}
