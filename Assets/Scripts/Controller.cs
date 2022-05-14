using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private float mSpeed = 300.0f;

    [SerializeField]
    private KeyCode mUpKey;
    [SerializeField]
    private KeyCode mDownKey;
    [SerializeField]
    private KeyCode mLeftKey;
    [SerializeField]
    private KeyCode mRightKey;

    private Rigidbody2D mRigidBody;

    private Vector2 mMove;
    
    // Start is called before the first frame update
    void Start()
    {
        mRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mMove = Vector2.zero;
        mMove += new Vector2(Input.GetKey(mRightKey) ? 1.0f : 0.0f,
                             Input.GetKey(mUpKey) ? 1.0f : 0.0f);
        mMove -= new Vector2(Input.GetKey(mLeftKey) ? 1.0f : 0.0f,
                             Input.GetKey(mDownKey) ? 1.0f : 0.0f);
        mRigidBody.AddForce(mMove * Time.deltaTime * mSpeed);
    }
}
