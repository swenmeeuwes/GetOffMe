using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public AnimationCurve rateOfSizeIncrease;
    public float SizeMultiplier = 1f;
    public float size = 1f;
    public void OnEnemyEnter(float size) {
        float increase = (size / 10);
        this.size += increase;
        transform.localScale += new Vector3(increase, increase, 0);
    }
}
