using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField]
    private GameObject mOther;

    private LineRenderer mLR;

    // Start is called before the first frame update
    void Start()
    {

        mLR = GetComponent<LineRenderer>();
        mLR.positionCount = 2;
        StartCoroutine("Connect");
    }

    IEnumerator Connect()
    {
        while (true) {
            mLR.SetPosition(0, transform.position);
            mLR.SetPosition(1, mOther.transform.position);
            yield return new WaitForFixedUpdate();
        }
    }
}
