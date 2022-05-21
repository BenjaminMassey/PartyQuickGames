using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField]
    private bool mRampUp = true;
    [SerializeField]
    private GameObject mBaseGO;
    [SerializeField]
    private float mFrequency;
    [SerializeField]
    private float mAmount;
    [SerializeField]
    private Vector3 mStart;
    [SerializeField]
    private Vector3 mVariance;

    private float mRampVal;
    private float mRNG;

    // Start is called before the first frame update
    void Start()
    {
        mRampVal = 0f;
        mRNG = 0f;
        StartCoroutine("Main");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Main() {
        while (true) {
            mRampVal = (mRampUp ? 1f : 0f) * (Time.timeSinceLevelLoad * 0.01f);
            mAmount += mRampVal;
            for (int _ = 0; _ < mAmount; _++) {
                Instantiate(mBaseGO);
                mRNG = Random.Range(-50.0f, 50.0f) / 100.0f;
                mBaseGO.transform.position = mStart + (mVariance * mRNG);
            }
            yield return new WaitForSeconds(mFrequency);
        }
    }
}
