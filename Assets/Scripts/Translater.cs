using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translater : MonoBehaviour
{
    [SerializeField]
    private float mSpeed = 50.0f;

    [SerializeField]
    private Vector3 mDirection;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        transform.Translate(mDirection * Time.deltaTime * mSpeed);
    }
}
