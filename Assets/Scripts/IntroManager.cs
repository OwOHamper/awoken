using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private int textIdx = 0;
    private int numOfTexts;
    public string[] introText;
    public TextMeshProUGUI text;

    public Animator textAnim;

    public float textPeriod = 5f;
    private float remainingTime;
    private bool changeText = false;


    private void Awake()
    {
        remainingTime = textPeriod;
        numOfTexts = introText.Length;
        text.text = introText[0];
    }

    private void Update()
    {
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
                        text.text = introText[textIdx];
                    }
                    else
                    {
                        textIdx += 1;
                        textAnim.SetTrigger("Fadeout");
                        remainingTime = 1f;
                        changeText = true;
                    }
                }
                else
                {
                    textIdx += 1;
                    text.gameObject.SetActive(false);
                    FindObjectOfType<LevelLoader>().LoadNextLevel(1);
                }
            }
            else
            {
                remainingTime -= Time.deltaTime;
            }
        }
    }
}
