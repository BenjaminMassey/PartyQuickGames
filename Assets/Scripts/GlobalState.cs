using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalState
{
    // what folder to grab characters from
    public static string path = "";

    // how many rounds to do, to be set by Selection (3 is legacy, but default good)
    public static int end = 3;

    // player values are joycon indices
    public static int[] players;

    // player value => character sprite
    public static Dictionary<int, Sprite> characters = new Dictionary<int, Sprite>();

    // player value => score
    public static Dictionary<int, int> scores = new Dictionary<int, int>();
}
