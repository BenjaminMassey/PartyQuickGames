using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    public int joyStickNum = 0;

    [SerializeField]
    private GameObject mBulletBase;
    [SerializeField]
    private float mBulletScale;
    [SerializeField]
    private float mBulletSpeed;
    [SerializeField]
    private float mShotRate = 2.0f;

    private Vector3 mTrueCenter;
    private GameState mGameState;
    private GameObject mPointA;
    private GameObject mPointB;
    private GameObject mLine;

    // Start is called before the first frame update
    void Start()
    {
        mTrueCenter = transform.position;
        mGameState = GameObject.Find("GameStateHandler").GetComponent<GameState>();
        mPointA = transform.Find("PointA").gameObject;
        mPointB = transform.Find("PointB").gameObject;
        mLine = transform.Find("Line").gameObject;

        transform.localPosition += (Vector3.right * 2.0f);

        StartCoroutine(KeepShooting());
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis(JoyCon.StickX(joyStickNum));
        float y = Input.GetAxis(JoyCon.StickY(joyStickNum));
        if (!(x == 0f && y == 0f))
        {
            transform.localPosition = new Vector3(y, x, 0f) * 2.0f;
            transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(x, y) * 180f / Mathf.PI);
        }
    }

    IEnumerator KeepShooting()
    {
        Color color = mLine.GetComponent<SpriteRenderer>().color;
        float start_time = Time.time;
        while (true)
        {
            float t = (Time.time - start_time) / mShotRate; // 0 => 1 cycle
            mLine.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, t);
            if (t >= 1f) 
            {
                start_time = Time.time;
                if (mGameState.GetRunning()) Shoot();
                mLine.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0f);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(mBulletBase);
        bullet.transform.GetChild(0).name = name + "'s Bullet";
        bullet.transform.parent = null;
        bullet.transform.localScale = Vector3.one * mBulletScale;
        bullet.transform.position = mPointA.transform.position;
        Vector3 dir = (mPointB.transform.position - bullet.transform.position).normalized;
        StartCoroutine(HandleShooting(bullet, dir));
    }

    IEnumerator HandleShooting(GameObject go, Vector3 dir)
    {
        while (true)
        {
            go.transform.position = go.transform.position + (dir * mBulletSpeed);
            yield return new WaitForFixedUpdate();
        }
    }
}
