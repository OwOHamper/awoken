using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMinigame : MonoBehaviour
{
    public GameObject dialogue;
    public GameObject musicMinigame;
    public GameObject[] notes;
    public int numOfRounds = 8;
    public float playTime = 0.45f;
    public bool clickable = false;
    private int guessIdx = 0;

    new public GameObject particleSystem;

    private List<GameObject> order = new List<GameObject>();
    private int roundIdx = 1;


    public void failMinigame()
    {
        // Debug.Log("fail");
        PlayerData.minigameMusicStatus = false;
        PlayerData.inMinigame = false;
        musicMinigame.SetActive(false);
        PlayerData.IncrementBatteryLevel(0);
    }

    public void passMinigame()
    {
        // Debug.Log("pass");
        PlayerData.minigameMusicStatus = true;
        PlayerData.inMinigame = false;
        musicMinigame.SetActive(false);
        PlayerData.IncrementBatteryLevel(0);
    }

    private void OnEnable()
    {
        PlayerData.canMove = false;
        FindObjectOfType<AudioManager>().Pause("Main theme");
        order = new List<GameObject>();
        roundIdx = 1;
        StartNextRound(1);
    }

    private void OnDisable()
    {
        FindObjectOfType<AudioManager>().UnPause("Main theme");
        PlayerData.canMove = true;
    }

    private void StartNextRound(int roundNum)
    {
        if (roundNum == numOfRounds+1)
        {
            passMinigame();
            return;
        }
        roundIdx = roundNum;
        guessIdx = 0;
        clickable = false;
        int noteNum = Random.Range(0, 11);
        order.Add(notes[noteNum]);
        StartCoroutine(PlayNote(0));
    }

    IEnumerator PlayNote(int noteIdx)
    {
        order[noteIdx].GetComponent<Note>().PlayNote();
        yield return new WaitForSeconds(playTime);
        order[noteIdx].GetComponent<Note>().StopPlaying();
        if (noteIdx < roundIdx - 1)
        {
            yield return new WaitForSeconds(.05f);
            StartCoroutine(PlayNote(noteIdx + 1));
        } else
        {
            clickable = true;
        }
    }

    public void ClickedNote(int noteIdx)
    {
        if (!clickable) return;
        clickable = false;
        bool fail = false, nextRound = false;
        guessIdx++;
        if (order[guessIdx-1] != notes[noteIdx])
            fail = true;
        else if (guessIdx == roundIdx)
            nextRound = true;

        StartCoroutine(PlayAnswer(noteIdx, fail, nextRound));
    }

    IEnumerator PlayAnswer(int noteIdx, bool fail, bool nextRound)
    {
        notes[noteIdx].GetComponent<Note>().PlayNote();
        if (!fail && !nextRound)
            clickable = true;
        yield return new WaitForSeconds(playTime);
        notes[noteIdx].GetComponent<Note>().StopPlaying();
        if (fail)
        {
            failMinigame();
        }
        else if (nextRound)
        {
            yield return new WaitForSeconds(0.3f);
            StartNextRound(roundIdx + 1);
        }
    }
}
