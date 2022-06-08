using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AutoBG : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (GlobalState.backgrounds.Count == 0)
            yield return StartCoroutine(Init());
        RandomBG();
    }

    IEnumerator Init()
    {
        foreach (string param in new string[] { "*.png", "*.jpg", "*.jpeg" })
        {
            yield return StartCoroutine(
                LoadImages(
                    Directory.GetFiles(GlobalState.backgrounds_path, param),
                    param
                )
            );
        }
    }

    void RandomBG()
    {
        int rng = Random.Range(0, GlobalState.backgrounds.Count);
        Sprite bg = GlobalState.backgrounds[rng];
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = bg;
        float aspect = ((float) bg.texture.height) / ((float) bg.texture.width);
        Vector2 size = new Vector2(3800f, 3800f * aspect);
        float scale_width = size.x / bg.texture.width;
        float scale_height = size.y / bg.texture.height;
        Vector3 initialScale = transform.localScale;
        transform.localScale = new Vector3(initialScale.x * scale_width,
                                                  initialScale.y * scale_height,
                                                  initialScale.z);
    }

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
                GlobalState.backgrounds.Add(s);
            }
        }
    }
}
