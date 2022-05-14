using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField]
    private bool mRampUp = true;
    [SerializeField]
    private float mSpeed = 50f;
    [SerializeField]
    private float mSwapFrequency = 10f;

    private float mStartTime;

    private float mRampVal;

    private float mDirection;

    private int mSwapCount;

    // Start is called before the first frame update
    void Start()
    {
        mStartTime = Time.time;
        mRampVal = 0f;
        mDirection = 1f;
        mSwapCount = 0;
        StartCoroutine("DirectionSwapper");
    }

    // Update is called once per frame
    void Update()
    {
        mRampVal = ((mRampUp ? 1f : 0f) + (mSwapCount / 4f)) * Mathf.Sqrt(Time.time - mStartTime);
        transform.Rotate(Vector3.forward * mDirection * Time.deltaTime * mSpeed * mRampVal);
    }

    IEnumerator DirectionSwapper()
    {
        while (true) {
            yield return new WaitForSeconds(mSwapFrequency);
            float prevDirection = mDirection;
            mDirection = 0f;
            yield return new WaitForSeconds(0.5f);
            mStartTime = Time.time;
            mDirection = prevDirection * -1f;
            mSwapCount++;
        }
    }
}
