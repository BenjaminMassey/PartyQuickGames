using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField]
    public GameObject mOther;

    private LineRenderer mLR;

    // Awake usage so done after PlayerGenerator does its thing
    void Awake()
    {
        // TODO: fix everything
        mLR = GetComponent<LineRenderer>();
        mLR.positionCount = 2;
        StartCoroutine("Connect");
    }

    IEnumerator Connect()
    {
        while (true) {
            if (mOther != null)
            {
                mLR.SetPosition(0, transform.position);
                mLR.SetPosition(1, mOther.transform.position);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
