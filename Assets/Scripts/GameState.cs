using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    [SerializeField]
    private GameObject mText;

    private List<GameObject> mPlayerGOs;

    private int mAlive;

    private List<GameObject> mDied;

    private Dictionary<int, int> mGlobalStateSnapshot;

    // Awake over start so that PlayerGenerator goes first
    void Awake()
    {
        Time.timeScale = 1f;
        bool over = false;
        foreach (int pNum in GlobalState.players)
        {
            if (!GlobalState.scores.ContainsKey(pNum))
                GlobalState.scores.Add(pNum, 0);
            else if (GlobalState.scores[pNum] >= GlobalState.end)
                over = true; // Could break, but wanna add any new players for end screen
        }
        if (over)
            StartCoroutine("Finale");
        else
        {
            mGlobalStateSnapshot = GlobalState.scores.ToDictionary(entry => entry.Key,
                                                                   entry => entry.Value);
            StartCoroutine("Begin");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine("Finale");
    }

    IEnumerator Begin()
    {
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
        mText.SetActive(true);
        mText.GetComponent<Text>().text = "Ready...";
        yield return new WaitForSecondsRealtime(1.85f);
        mText.GetComponent<Text>().text = "GO!";
        yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale = prevTimeScale;
        mText.SetActive(false);
        mPlayerGOs = GameObject.FindGameObjectsWithTag("Player").ToList();
        mAlive = mPlayerGOs.Count;
        mDied = new List<GameObject>();
        Events.PlayerDeath.AddListener(PlayerDied);
    }

    void PlayerDied(GameObject died) {
        mAlive--;
        mDied.Add(died);
        if (mAlive == 1)
        {
            int winner = -1;
            foreach (GameObject player in mPlayerGOs)
            {
                if (!mDied.Contains(player))
                    winner = player.GetComponent<Controller>().joyStickNum;
            }
            End(winner);
        }
        else if (mAlive <= 0)
        {
            StopAllCoroutines();
            foreach (KeyValuePair<int, int> score in mGlobalStateSnapshot)
                GlobalState.scores[score.Key] = score.Value;
            End(-1);
        }
            
    }

    void End(int winner)
    {
        if (GlobalState.scores.ContainsKey(winner)) GlobalState.scores[winner]++;
        string msg = "";
        msg += winner == -1 ? "TIE!\n\n" :
                              GlobalState.characters[winner].name
                                    + " won the round!\n\n";
        foreach (KeyValuePair<int, int> score in GlobalState.scores)
            msg += GlobalState.characters[score.Key].name + 
                        ": " + score.Value.ToString() + "\n";
        mText.GetComponent<Text>().text = msg;
        mText.SetActive(true);
        StartCoroutine("NextRound");
    }

    IEnumerator NextRound()
    {
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = prevTimeScale;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int rng = UnityEngine.Random.Range(2, sceneCount);
        SceneManager.LoadScene(rng);
    }

    IEnumerator Finale() 
    {
        string end = "Rounds are over...\n\n";
        int winner = -1;
        int high_score = -1;
        KeyValuePair<int, int>[] charArray = GlobalState.scores.ToArray();
        foreach (KeyValuePair<int, int> score in GlobalState.scores)
        {
            end += GlobalState.characters[score.Key].name + 
                        ": " + score.Value.ToString() + "\n";
            if (score.Value > high_score)
            {
                winner = score.Key;
                high_score = score.Value;
            } 
        }
        end += "\nCongrats to " + GlobalState.characters[winner].name + "!";
        end += "\n\nPress the + or - button to restart.";
        mText.SetActive(true);
        mText.GetComponent<Text>().text = end;
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        while (true)
        {
            if (Input.GetKey(JoyCon.Plus()) || Input.GetKey(JoyCon.Minus()))
                break;
            yield return new WaitForEndOfFrame();
        }
        Time.timeScale = prevTimeScale;
        GlobalState.scores.Clear();
        GlobalState.characters.Clear();
        SceneManager.LoadScene(1);
    }
}
