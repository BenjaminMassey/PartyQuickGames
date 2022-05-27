using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalState
{
    // how many rounds to do, to be set by Selection (3 is legacy, but default good
    public static int end = 3;

    // player values are joycon indices
    public static int[] players;

    // player value (GlobalState.players) => character sprite (image)
    public static Dictionary<int, Sprite> characters = new Dictionary<int, Sprite>();

    // player name (string) => score (int)
    public static Dictionary<string, int> scores = new Dictionary<string, int>();
}
