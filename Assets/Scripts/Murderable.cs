using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murderable : MonoBehaviour
{
    private bool mDying;

    // Start is called before the first frame update
    void Start()
    {
        mDying = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag.Equals("Murderer") && !mDying)
            Death();
    }

    void Death() {
        mDying = true; // Shouldn't be necessary but here we are (events are weird)
        Events.PlayerDeath.Invoke(gameObject);
        Destroy(gameObject);
    }
}
