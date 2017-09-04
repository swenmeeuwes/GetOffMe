using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GamePhase : ScriptableObject
{
    public int time;
    public List<GameObject> objectKeys;
    public List<float> percentages;
}
