using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using static System.Environment;
using UnityEngine.SceneManagement;

public class FolderSelecting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FileBrowser.ShowLoadDialog(Selected,
                                   Canceled,
                                   SimpleFileBrowser.FileBrowser.PickMode.Folders,
                                   false, // multi-select
                                   GetFolderPath(SpecialFolder.UserProfile));
    }

    private void Selected(string[] paths)
    {
        GlobalState.path = paths[0];
        SceneManager.LoadScene(1);
    }

    void Canceled()
    { 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
