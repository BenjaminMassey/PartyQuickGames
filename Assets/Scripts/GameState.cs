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
    private List<GameObject> mPlayers;

    [SerializeField]
    private GameObject mText;

    private int mAlive;

    private List<GameObject> mDied;

    private Dictionary<string, int> mGlobalStateSnapshot;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        bool over = false;
        foreach (GameObject player in mPlayers)
        {
            if (!GlobalState.scores.ContainsKey(player.name))
                GlobalState.scores.Add(player.name, 0);
            else if (GlobalState.scores[player.name] >= GlobalState.end)
                over = true; // Could break, but wanna add any new players for end screen
            else
                Debug.Log(player.name + ": " + GlobalState.scores[player.name]);
        }
        if (over)
            Finale();
        else
        {
            mGlobalStateSnapshot = GlobalState.scores.ToDictionary(entry => entry.Key,
                                                                   entry => entry.Value);
            StartCoroutine("Begin");
        }
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
        mAlive = mPlayers.Count;
        mDied = new List<GameObject>();
        Events.PlayerDeath.AddListener(PlayerDied);
    }

    void PlayerDied(GameObject died) {
        mAlive--;
        mDied.Add(died);
        if (mAlive == 1)
        {
            string winner = "";
            foreach (GameObject player in mPlayers)
            {
                if (!mDied.Contains(player))
                    winner = player.name;
            }
            End(winner);
        }
        else if (mAlive <= 0)
        {
            StopAllCoroutines();
            foreach (KeyValuePair<string, int> score in mGlobalStateSnapshot)
                GlobalState.scores[score.Key] = score.Value;
            End(string.Empty);
        }
            
    }

    void End(string winner)
    {
        mText.GetComponent<Text>().text = winner == string.Empty ? "TIE!" : winner + " won the game!";
        mText.SetActive(true);
        if (GlobalState.scores.ContainsKey(winner)) GlobalState.scores[winner]++;
        StartCoroutine("NextRound");
    }

    IEnumerator NextRound()
    {
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(3.0f);
        Time.timeScale = prevTimeScale;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int rng = UnityEngine.Random.Range(0, sceneCount);
        SceneManager.LoadScene(rng);
    }

    void Finale() 
    {
        string end = "";
        string winner = "";
        int high_score = -1;
        foreach (KeyValuePair<string, int> score in GlobalState.scores)
        {
            end += score.Key + ": " + score.Value.ToString() + "\n";
            if (score.Value > high_score) winner = score.Key;
        }
        end += "Congrats to " + winner + "!";
        mText.SetActive(true);
        mText.GetComponent<Text>().text = end;

        GlobalState.scores.Clear();
        StartCoroutine("NextRound");
    }
}
