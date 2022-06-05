using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private List<Sprite> mCharacterSprites;

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

    private bool mLoaded = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        mTextObj.SetActive(true);
        mTextObj.GetComponent<Text>().text = "Loading images...";
        mCharacterSprites = new List<Sprite>();
        foreach (string param in new string[] { "*.png", "*.jpg", "*.jpeg" })
        {
            yield return StartCoroutine(
                LoadImages(
                    Directory.GetFiles(GlobalState.characters_path, param),
                    param
                )
            );
        }
        mInitialScale = mImageObj.transform.localScale;
        mTextObj.SetActive(true);
        mCurrent = Setting.Rounds;
        mHeader = new Dictionary<Setting, string>()
        {
            { Setting.Rounds, "Select the number of rounds." },
            { Setting.Players, "Press A/B/X/Y to join the game." },
            { Setting.Characters, "Select your character!" }
        };
        mPlayerValues = new List<int>();
        mNum = 0;
        mCurrentPlayerIndex = 0;
        mLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mLoaded) return;

        if (Input.GetKeyDown(JoyCon.Plus()) ||
            Input.GetKeyDown(JoyCon.Minus()) ||
            Input.GetKeyDown(KeyCode.Return))
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
                if (Input.GetKeyDown(KeyCode.RightArrow)) mNum += 1;
                if (Input.GetKeyDown(KeyCode.LeftArrow)) mNum -= 1;
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
                        Input.GetKeyDown(JoyCon.X(i)) || Input.GetKeyDown(JoyCon.Y(i)) ||
                        (Input.GetKeyDown(KeyCode.Space) && i == 1))
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
                    (Input.GetKeyDown(JoyCon.A(GlobalState.players[mCurrentPlayerIndex])) ||
                     Input.GetKeyDown(JoyCon.B(GlobalState.players[mCurrentPlayerIndex])) ||
                     Input.GetKeyDown(JoyCon.X(GlobalState.players[mCurrentPlayerIndex])) ||
                     Input.GetKeyDown(JoyCon.Y(GlobalState.players[mCurrentPlayerIndex])) ||
                     Input.GetKeyDown(KeyCode.Space)) &&
                    !GlobalState.characters.ContainsValue(mCharacterSprites[mNum]))
                {
                    GlobalState.characters.Add(GlobalState.players[mCurrentPlayerIndex], mCharacterSprites[mNum]);
                    mCurrentPlayerIndex++;
                    mNum = -1;
                }
                else
                {
                    if (mCurrentPlayerIndex < GlobalState.players.Length)
                    {
                        int prevNum = mNum;
                        mNum += Input.GetKeyDown(JoyCon.SR(GlobalState.players[mCurrentPlayerIndex])) ? 1 : 0;
                        mNum -= Input.GetKeyDown(JoyCon.SL(GlobalState.players[mCurrentPlayerIndex])) ? 1 : 0;
                        if (Input.GetKeyDown(KeyCode.RightArrow)) mNum += 1;
                        if (Input.GetKeyDown(KeyCode.LeftArrow)) mNum -= 1;
                        if (mNum <= -1) mNum = mCharacterSprites.Count - 1;
                        if (mNum >= mCharacterSprites.Count) mNum = 0;
                        if (mNum != prevNum || prevNum == -1)
                        {
                            mImageObj.GetComponent<SpriteRenderer>().sprite = mCharacterSprites[mNum];
                            if (GlobalState.characters.ContainsValue(mCharacterSprites[mNum]))
                                mImageObj.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f);
                            else
                                mImageObj.GetComponent<SpriteRenderer>().color = Color.white;
                            float scale_width = mImgSize.x / mCharacterSprites[mNum].texture.width;
                            float scale_height = mImgSize.y / mCharacterSprites[mNum].texture.height;
                            mImageObj.transform.localScale = new Vector3(mInitialScale.x * scale_width,
                                                                         mInitialScale.y * scale_height,
                                                                         mInitialScale.z);
                            string name = mCharacterSprites[mNum].name;
                            string msg = mHeader[mCurrent] + " (P" +  mCurrentPlayerIndex.ToString()+ ")\n" + 
                                            "SL + SR to change, A/B/X/Y to select.\n\n" + name + "\n\n\n\n\n\n\n\n";
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

    // https://forum.unity.com/threads/loading-image-files-from-a-folder-on-desktop.206201/#post-1393568
    public IEnumerator LoadImages(string[] filePaths, string param)
    {
        foreach (string filePath in filePaths)
        {
            WWW load = new WWW("file:///" + filePath); // TODO: upgrade to UnityWebRequest
            yield return load;
            if (!string.IsNullOrEmpty(load.error))
                Debug.LogWarning(filePath + " error");
            else
            {
                Sprite s = Sprite.Create(load.texture,
                                      new Rect(0, 0, load.texture.width, load.texture.height),
                                      new Vector2(0.5f, 0.5f));
                string[] pieces = filePath.Split('\\');
                string name = pieces[pieces.Length - 1];
                name = name.Substring(0, (name.Length - param.Length) + 1);
                name = name.Replace('_', ' ');
                s.name = name;
                mCharacterSprites.Add(s);
            }
        }
    }

    void End()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int rng = UnityEngine.Random.Range(2, sceneCount);
        SceneManager.LoadScene(rng);
    }
}
