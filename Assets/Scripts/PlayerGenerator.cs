using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject mPlayerBase;

    private Vector2 mSize = new Vector2(200f, 200f);

    private Vector3[] mStarts;

    // Start is called before the first frame update
    void Start()
    {
        mStarts = new Vector3[]
        {
            new Vector3(-5f, -5f, 0f),
            new Vector3(0f, -5f, 0f),
            new Vector3(5f, -5f, 0f),
            new Vector3(-5f, 0f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(5f, 0f, 0f),
            new Vector3(-5f, 5f, 0f),
            new Vector3(5f, 5f, 0f)
        };
        for (int i = 0; i < GlobalState.players.Length; i++)
        {
            GameObject player = Instantiate(mPlayerBase);
            player.GetComponent<Controller>().joyStickNum = GlobalState.players[i];
            player.transform.position = mStarts[i];
            Sprite sprite = GlobalState.characters[GlobalState.players[i]];
            player.name = sprite.name.Replace('_', ' ');
            player.GetComponent<SpriteRenderer>().sprite = sprite;
            float scale_width = mSize.x / GlobalState.characters[GlobalState.players[i]].texture.width;
            float scale_height = mSize.y / GlobalState.characters[GlobalState.players[i]].texture.height;
            Vector3 initialScale = player.transform.localScale;
            player.transform.localScale = new Vector3(initialScale.x * scale_width,
                                                      initialScale.y * scale_height,
                                                      initialScale.z);
            float factor = mSize.x / 100f; // 100 seems to be magic starter for player (assumed square)
            float lax = 1f;
            player.GetComponent<BoxCollider2D>().size = new Vector2((factor * lax) / scale_width, (factor * lax) / scale_height);
        }
    }
}
