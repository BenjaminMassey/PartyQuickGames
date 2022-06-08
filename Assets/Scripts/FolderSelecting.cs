using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using static System.Environment;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FolderSelecting : MonoBehaviour
{
    [SerializeField]
    private GameObject text_obj;

    private enum FolderType
    {
        Backgrounds,
        Characters
    };

    private FolderType mCurrent;

    private FileBrowser mFileBrowser;

    private int mFileIndex;

    private bool mNeutraled;

    // Start is called before the first frame update
    void Start()
    {
        text_obj.SetActive(true);
        mCurrent = FolderType.Backgrounds;
        MakeDialog("Choose a folder of backgrounds.");
        mFileBrowser = GameObject.Find("SimpleFileBrowserCanvas(Clone)").GetComponent<FileBrowser>();
        mFileIndex = 0;
        mFileBrowser.SelectIndex(0);
        mNeutraled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(JoyCon.X()))
        {
            mFileBrowser.OnItemSelected(mFileBrowser.GetCurrentItem(), true);
            mFileIndex = 0;
            mFileBrowser.SelectIndex(0);
        }

        if (Input.GetKeyDown(JoyCon.Plus()))
            mFileBrowser.OnSubmitButtonClicked();

        if (Input.GetKeyDown(JoyCon.A()))
        {
            mFileBrowser.OnUpButtonPressed();
            mFileIndex = 0;
            mFileBrowser.SelectIndex(0);
        }

        int change = 0;
        float stick = Input.GetAxis(JoyCon.StickX());
        if (mNeutraled && (stick > 0.5f || stick < -0.5f))
        {
            mNeutraled = false;
            change += (Mathf.RoundToInt(stick) * -1);
        }
        if (stick == 0) mNeutraled = true;

        if (change == 0) return;

        mFileIndex = mFileIndex + change;
        if (mFileIndex < 0) mFileIndex = 0;
        int vfec = mFileBrowser.GetValidFileEntriesCount();
        if (mFileIndex >= vfec) mFileIndex = vfec - 1;
        
        mFileBrowser.SelectIndex(mFileIndex);

        mFileBrowser.SetScrollVector(((float) vfec - mFileIndex) / ((float) vfec));
    }

    void MakeDialog(string top_text)
    {
        top_text += "\nStick = move  ;  X / > = select  ;  A / \\/ = move  ;  + / - = confirm.";
        text_obj.GetComponent<Text>().text = top_text;
        FileBrowser.ShowLoadDialog(Selected,
                                   Canceled,
                                   SimpleFileBrowser.FileBrowser.PickMode.Folders,
                                   false, // multi-select
                                   GetFolderPath(SpecialFolder.UserProfile));
        Vector3 pos = GameObject.Find("SimpleFileBrowserWindow").transform.position;
        GameObject.Find("SimpleFileBrowserWindow").transform.position = new Vector3(pos.x, -1f, pos.z);
    }

    private void Selected(string[] paths)
    {
        if (mCurrent == FolderType.Backgrounds)
        {
            GlobalState.backgrounds_path = paths[0];
            mCurrent = FolderType.Characters;
            MakeDialog("Choose a folder of characters.");
        }
        else if (mCurrent == FolderType.Characters)
        {
            GlobalState.characters_path = paths[0];
            SceneManager.LoadScene(1);
        }
    }

    void Canceled()
    { 
        
    }
}
