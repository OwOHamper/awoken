using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;


    public void LoadNextLevel(int idx)
    {
        StartCoroutine(LoadLevel(idx));
    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        PlayerData.canMove = true;
        SceneManager.LoadScene(LevelIndex);
    }
}
