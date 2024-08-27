using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingsManager : MonoBehaviour
{
    public Ending[] endings;
    private string musicName;

    public float textPeriod = 5f;
    private float remainingTime;
    private bool canLeave = false;
    private int textIdx = 0;
    private string[] endingTexts;
    private int numOfTexts;

    public Animator textAnim;
    public TextMeshProUGUI text;
    private bool changeText = false;

    public GameObject credits;
    public GameObject creditsBG;

    private void Awake()
    {
        remainingTime = textPeriod;
        if (PlayerData.endingIndex < 0)
        {
            Debug.LogError("No ending set!\nEnding num: " + PlayerData.endingIndex);
            return;
        }

        Ending ending = endings[PlayerData.endingIndex];
        int endingAmount = PlayerPrefs.GetInt(ending.name, 0);
        PlayerPrefs.SetInt(ending.name, endingAmount+1);
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.Stop("Main theme");
        ending.endingObject.SetActive(true);
        audioManager.Play(ending.musicName);
        musicName = ending.musicName;
        endingTexts = ending.endingText;
        numOfTexts = endingTexts.Length;
        text.text = endingTexts[0];
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canLeave)
        {
            Application.Quit();
        }

        if (textIdx <= numOfTexts)
        {
            if (remainingTime <= 0)
            {
                if (textIdx < numOfTexts)
                {
                    if (changeText)
                    {
                        changeText = false;
                        remainingTime = textPeriod;
                        text.text = endingTexts[textIdx];
                    }
                    else
                    {
                        textIdx += 1;
                        textAnim.SetTrigger("Fadeout");
                        remainingTime = 1f;
                        changeText = true;
                    }
                } else
                {
                    textIdx += 1;
                    credits.SetActive(true);
                    creditsBG.SetActive(true);
                    text.gameObject.SetActive(false);
                    StartCoroutine(WatchCredits());
                }
            } else
            {
                remainingTime -= Time.deltaTime;
            }
        }
    }

    IEnumerator WatchCredits()
    {
        yield return new WaitForSeconds(10f);
        canLeave = true;
    }
}

