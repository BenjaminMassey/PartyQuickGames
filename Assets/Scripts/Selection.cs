using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    [SerializeField]
    private GameObject mTextObj;
    [SerializeField]
    private GameObject mImageObj;

    private Vector2 mImgSize = new Vector2(400f, 400f);

    private Sprite[] mCharacterSprites;

    private Vector3 mInitialScale;

    private enum Setting
    { 
      Rounds,
      Players,
      Characters
    };

    private Setting mCurrent;

    private Dictionary<Setting, string> mHeader;

    private List<int> mPlayerValues;

    private int mNum;

    private int mCurrentPlayerIndex;

    // Start is called before the first frame update
    void Start()
    {
        mCharacterSprites = Resources.LoadAll<Sprite>("Characters");
        mInitialScale = mImageObj.transform.localScale;
        mTextObj.SetActive(true);
        mCurrent = Setting.Rounds;
        mHeader = new Dictionary<Setting, string>()
        {
            { Setting.Rounds, "Select the number of rounds." },
            { Setting.Players, "Press a face button to join the game." },
            { Setting.Characters, "Select your character." }
        };
        mPlayerValues = new List<int>();
        mNum = 0;
        mCurrentPlayerIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(JoyCon.Plus()) ||
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
                GlobalState.players = mPlayerValues.ToArray();
                mImageObj.SetActive(true);
                mNum = -1; // silly workaround for initial update
                mCurrent = Setting.Characters;
            }
            else if (mCurrent == Setting.Characters)
            {
                if (GlobalState.players.Length == GlobalState.characters.Count)
                    End();
            }
        }
        else 
        {
            if (mCurrent == Setting.Rounds)
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
            else if (mCurrent == Setting.Players)
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (Input.GetKeyDown(JoyCon.A(i)) || Input.GetKeyDown(JoyCon.B(i)) ||
                        Input.GetKeyDown(JoyCon.X(i)) || Input.GetKeyDown(JoyCon.Y(i)))
                    {
                        if (!mPlayerValues.Contains(i))
                        {
                            mPlayerValues.Add(i);
                            mNum++;
                        }
                    }
                }
                string msg = mHeader[mCurrent] + "\n";
                msg += "Number of Players: " + mNum.ToString();
                mTextObj.GetComponent<Text>().text = msg;
            }
            else if (mCurrent == Setting.Characters)
            {
                if (mCurrentPlayerIndex < GlobalState.players.Length &&
                    Input.GetKeyDown(JoyCon.X(GlobalState.players[mCurrentPlayerIndex])))
                {
                    GlobalState.characters.Add(GlobalState.players[mCurrentPlayerIndex], mCharacterSprites[mNum]);
                    mCurrentPlayerIndex++;
                }
                else
                {
                    if (mCurrentPlayerIndex < GlobalState.players.Length)
                    {
                        int prevNum = mNum;
                        mNum += Input.GetKeyDown(JoyCon.SR(GlobalState.players[mCurrentPlayerIndex])) ? 1 : 0;
                        mNum -= Input.GetKeyDown(JoyCon.SL(GlobalState.players[mCurrentPlayerIndex])) ? 1 : 0;
                        if (mNum <= -1) mNum = mCharacterSprites.Length - 1;
                        if (mNum >= mCharacterSprites.Length) mNum = 0;
                        if (mNum != prevNum || mNum == -1)
                        {
                            mImageObj.GetComponent<SpriteRenderer>().sprite = mCharacterSprites[mNum];
                            float scale_width = mImgSize.x / mCharacterSprites[mNum].texture.width;
                            float scale_height = mImgSize.y / mCharacterSprites[mNum].texture.height;
                            mImageObj.transform.localScale = new Vector3(mInitialScale.x * scale_width,
                                                                         mInitialScale.y * scale_height,
                                                                         mInitialScale.z);
                            string name = mCharacterSprites[mNum].texture.name;
                            name = name.Replace('_', ' ');
                            string msg = mHeader[mCurrent] + "\n" + name + "\n\n\n\n\n\n\n";
                            mTextObj.GetComponent<Text>().text = msg;
                        }
                    }
                    else
                    {
                        mImageObj.SetActive(false);
                        mTextObj.GetComponent<Text>().text = "All players have selected their characters.";
                    }
                }
            }
        }
    }

    void End()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int rng = UnityEngine.Random.Range(1, sceneCount);
        SceneManager.LoadScene(rng);
    }
}
