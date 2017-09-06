using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreParticleManager : MonoBehaviour {
    [SerializeField]
    private GameObject rewardIndicatorPrefab;

    public void ShowRewardIndicatorAt(int scoreAmount, Vector3 worldPosition, bool clampInsideView = false)
    {
        var screenPostion = Camera.main.WorldToScreenPoint(worldPosition);
        if (clampInsideView)
        {
            var offset = 32;
            screenPostion = new Vector3(
                Mathf.Clamp(screenPostion.x, 0 + offset, Camera.main.pixelWidth - offset),
                Mathf.Clamp(screenPostion.y, 0 + offset, Camera.main.pixelHeight - offset),
                screenPostion.z
            );
        }

        var rewardIndicatorObject = Instantiate(rewardIndicatorPrefab);
        rewardIndicatorObject.transform.position = screenPostion;

        var rewardIndicator = rewardIndicatorObject.GetComponent<ScoreRewardIndicator>();
        rewardIndicator.Text = string.Format("+{0}", scoreAmount);

        rewardIndicatorObject.transform.SetParent(transform, true);
    }
}
