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

    // Start is called before the first frame update
    void Start()
    {
        mCurrent = FolderType.Backgrounds;
        MakeDialog("Choose a folder of backgrounds.");
    }

    void MakeDialog(string top_text)
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
