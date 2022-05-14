using System;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
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
            foreach (GameObject player in mPlayers) {
                if (!mDied.Contains(player))
                    winner = player.name;
            }
            End(winner + " won the game!");
        }
        else if (mAlive <= 0)
            End("Tie!");
    }

    void End(string message) {
        mText.GetComponent<Text>().text = message;
        mText.SetActive(true);
        StartCoroutine("NextRound");
    }

    IEnumerator NextRound() {
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(3.0f);
        Time.timeScale = prevTimeScale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
