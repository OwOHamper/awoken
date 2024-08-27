using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingLoader : MonoBehaviour
{

    public void LoadEnding(int endingIdx)
    {
        PlayerData.endingIndex = endingIdx;
        FindObjectOfType<LevelLoader>().LoadNextLevel(8);
    }
}
