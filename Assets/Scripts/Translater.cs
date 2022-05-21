using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translater : MonoBehaviour
{
    [SerializeField]
    private bool mRampUp = true;
    [SerializeField]
    private float mSpeed = 50.0f;
    [SerializeField]
    private Vector3 mDirection;

    private float mRampVal;
    private Vector4 mBounds;

    // Start is called before the first frame update
    void Start() 
    {
        mRampVal = 0f;
        GameObject bounds = GameObject.Find("Bounds");
        if (bounds)
        {
            float yMax = bounds.transform.Find("Up").transform.position.y;
            yMax += transform.localScale.y;
            float yMin = bounds.transform.Find("Down").transform.position.y;
            yMin -= transform.localScale.y;
            float xMax = bounds.transform.Find("Right").transform.position.x;
            xMax += transform.localScale.x;
            float xMin = bounds.transform.Find("Left").transform.position.x;
            xMin -= transform.localScale.x;
            mBounds = new Vector4(yMax, yMin, xMax, xMin);
        }
        else 
        {
            float yMax = transform.position.y + 100f;
            float yMin = transform.position.y - 100f;
            float xMax = transform.position.x + 100f;
            float xMin = transform.position.x - 100f;
            mBounds = new Vector4(yMax, yMin, xMax, xMin);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckBounds();
        mRampVal = mRampUp ? (Time.timeSinceLevelLoad * 0.75f) : 1f;
        transform.Translate(mDirection * Time.deltaTime * mSpeed * mRampVal);
    }

    void CheckBounds()
    {
        if (transform.position.y > mBounds[0] ||
            transform.position.y < mBounds[1] ||
            transform.position.x > mBounds[2] ||
            transform.position.x < mBounds[3])
            Destroy(gameObject);
    }
}
