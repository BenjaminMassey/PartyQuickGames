using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class Events
{
    // PlayerDeath includes which player has died as GameObject parameter
    public static UnityEvent<GameObject> PlayerDeath = new UnityEvent<GameObject>();
}
