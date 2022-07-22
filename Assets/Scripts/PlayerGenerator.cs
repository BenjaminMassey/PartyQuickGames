using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject mPlayerBase;
    [SerializeField]
    private GameObject mSpikeBase;
    [SerializeField]
    private GameObject mGunBase;

    private Vector2 mSize = new Vector2(200f, 200f);

    private Vector3[] mStarts;

    // Start is called before the first frame update
    void Start()
    {
        mStarts = new Vector3[]
        {
            new Vector3(-5f, -5f, 0f),
            new Vector3(5f, -5f, 0f),
            new Vector3(0f, -5f, 0f),
            new Vector3(-5f, 0f, 0f),
            new Vector3(5f, 0f, 0f),
            new Vector3(-5f, 5f, 0f),
            new Vector3(5f, 5f, 0f),
            new Vector3(0f, 0f, 0f)
        };
        bool debug_override = false;
        int n = 0;
        try { n = GlobalState.players.Length; }
        catch (System.Exception e) { Debug.Log(e.Message); }
        if (n == 0)
        {
            n = 1;
            debug_override = true;
        }
        for (int i = 0; i < n; i++)
        {
            GameObject player = Instantiate(mPlayerBase);
            player.transform.position = mStarts[i];
            if (!debug_override)
            {
                player.GetComponent<Controller>().joyStickNum = GlobalState.players[i];
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
            else
                player.GetComponent<Controller>().joyStickNum = -1; // handled by Controller
            
            if (mSpikeBase != null)
            {
                GameObject spike = Instantiate(mSpikeBase);
                spike.name = player.name + "'s Spike";
                spike.transform.position = player.transform.position + (Vector3.right * 2f);
                spike.GetComponent<Connector>().mOther = player;
                if (!debug_override) spike.GetComponent<Controller>().joyStickNum = GlobalState.players[i];
                else spike.GetComponent<Controller>().joyStickNum = -1;
                HingeJoint2D hj = player.AddComponent<HingeJoint2D>();
                hj.connectedBody = spike.GetComponent<Rigidbody2D>();
            }
            if (mGunBase != null)
            {
                GameObject gun = Instantiate(mGunBase);
                gun.name = player.name + "'s Gun";
                gun.transform.position = player.transform.position;
                gun.transform.parent = player.transform;
                if (!debug_override) gun.GetComponent<Gunner>().joyStickNum = GlobalState.players[i];
                else gun.GetComponent<Gunner>().joyStickNum = -1;
            }
        }
    }
}
