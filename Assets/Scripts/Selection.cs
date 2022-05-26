using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    [SerializeField]
    private GameObject mTextObj;

    private enum Setting
    { 
      Rounds,
      Players
    };

    private Setting mCurrent;

    private Dictionary<Setting, string> mHeader;

    private int mNum;

    // Start is called before the first frame update
    void Start()
    {
        mTextObj.SetActive(true);
        mCurrent = Setting.Rounds;
        mHeader = new Dictionary<Setting, string>()
        {
            { Setting.Rounds, "Select the number of rounds." },
            { Setting.Players, "Select the number of players." }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(JoyCon.X()) || 
            Input.GetKeyDown(JoyCon.Plus()) ||
            Input.GetKeyDown(JoyCon.Minus()))
        {
            if (mCurrent == Setting.Rounds)
            {
                mNum = Mathf.Max(mNum, 1);
                GlobalState.end = mNum;
                mNum = 0;
                mCurrent = Setting.Players;
            }
            else if (mCurrent == Setting.Players)
            {
                // TODO: actual stuff
                End();
            }
        }
        else 
        {
            mNum += Input.GetKeyDown(JoyCon.SR()) ? 1 : 0;
            mNum -= Input.GetKeyDown(JoyCon.SL()) ? 1 : 0;
            mNum = Mathf.Max(mNum, 0);
            string msg = mHeader[mCurrent] + "\n\n";
            if (mNum > 0) msg += "< ";
            else msg += "  ";
            msg += mNum.ToString();
            msg += " >";
            mTextObj.GetComponent<Text>().text = msg;
        }
    }

    void End()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int rng = UnityEngine.Random.Range(1, sceneCount);
        SceneManager.LoadScene(rng);
    }
}
