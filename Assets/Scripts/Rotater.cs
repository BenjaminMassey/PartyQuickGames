using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField]
    private float mSpeed = 50.0f;

    private Vector3 mDirection;

    // Start is called before the first frame update
    void Start()
    {
        mDirection = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(mDirection * Time.deltaTime * mSpeed);
    }
}
