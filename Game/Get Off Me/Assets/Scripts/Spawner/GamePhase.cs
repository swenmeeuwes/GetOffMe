using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePhase : ScriptableObject
{
    public int time;
    public List<GameObject> objectKeys;
    public List<float> weights;
}
