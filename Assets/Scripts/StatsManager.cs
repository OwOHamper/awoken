using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public int endingsCount = 8;
    public Sprite cross;
    public Sprite tick;

    private void OnEnable()
    {
        ReloadEndings();
    }

    public void ResetStats()
    {
        for (int i = 0; i < endingsCount; i++)
        {
            string ending = "Ending" + i;
            PlayerPrefs.DeleteKey(ending);
        }
        ReloadEndings();
    }

    private void ReloadEndings()
    {
        for (int i = 0; i < endingsCount; i++)
        {
            string ending = "Ending" + i;
            Image tickOrCross = GameObject.Find(ending).GetComponentInChildren<Image>();
            tickOrCross.sprite = PlayerPrefs.GetInt(ending, 0) > 0 ? tick : cross;
        }
    }
}
