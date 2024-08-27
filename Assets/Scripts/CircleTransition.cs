using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTransition : MonoBehaviour
{
    public float transitionTime = 1.5f;

    private void Awake()
    {
        if (PlayerData.loadingFirstTime)
        {
            StartCoroutine(StartTransition());
        } else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator StartTransition()
    {
        PlayerData.loadingFirstTime = false;
        gameObject.GetComponentInChildren<Animator>().SetTrigger("Circle");
        yield return new WaitForSeconds(transitionTime);
        gameObject.SetActive(false);
    }
}
