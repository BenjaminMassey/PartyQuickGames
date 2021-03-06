using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public int joyStickNum;

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
        if (Input.GetKeyDown(KeyCode.J))
        {
            string[] JSNs = Input.GetJoystickNames();
            Debug.Log("\n--- Joystick Names Start ---");
            foreach (string JSN in JSNs) 
            {
                Debug.Log("JSN: " + JSN);
            }
            Debug.Log("--- Joystick Names End ---\n");
        }

        mMove = Vector2.zero;
        mMove += new Vector2(Input.GetKey(mRightKey) ? 1f : 0f,
                             Input.GetKey(mUpKey) ? 1f : 0f);
        mMove -= new Vector2(Input.GetKey(mLeftKey) ? 1f : 0f,
                             Input.GetKey(mDownKey) ? 1f : 0f);
        if (joyStickNum > -1)
        {
            mMove += new Vector2(Input.GetAxis(JoyCon.StickY(joyStickNum)), Input.GetAxis(JoyCon.StickX(joyStickNum)));
            mMove += new Vector2(Input.GetKey(JoyCon.X(joyStickNum)) ? 1f : 0f,
                                 Input.GetKey(JoyCon.Y(joyStickNum)) ? 1f : 0f);
            mMove -= new Vector2(Input.GetKey(JoyCon.B(joyStickNum)) ? 1f : 0f,
                                 Input.GetKey(JoyCon.A(joyStickNum)) ? 1f : 0f);
        }
        else
        {
            mMove += new Vector2(Input.GetAxis(JoyCon.StickY()), Input.GetAxis(JoyCon.StickX()));
            mMove += new Vector2(Input.GetKey(JoyCon.X()) ? 1f : 0f,
                                 Input.GetKey(JoyCon.Y()) ? 1f : 0f);
            mMove -= new Vector2(Input.GetKey(JoyCon.B()) ? 1f : 0f,
                                 Input.GetKey(JoyCon.A()) ? 1f : 0f);
        }
        mMove = new Vector2(Mathf.Clamp(mMove.x, -1f, 1f), Mathf.Clamp(mMove.y, -1f, 1f));
        mRigidBody.AddForce(mMove * Time.deltaTime * mSpeed);
    }
}
