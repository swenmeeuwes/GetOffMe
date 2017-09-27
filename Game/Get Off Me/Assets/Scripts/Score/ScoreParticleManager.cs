using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreParticleManager : MonoBehaviour {
    [SerializeField]
    private GameObject rewardIndicatorPrefab;
    private ComboSystem comboSystem;

    private void Start()
    {
        comboSystem = FindObjectOfType<ComboSystem>();
    }

    public void ShowRewardIndicatorAt(int scoreAmount, Vector3 worldPosition, bool clampInsideView = false)
    {
        var screenPostion = Camera.main.WorldToScreenPoint(worldPosition);
        if (clampInsideView)
        {
            var offset = 32;
            screenPostion = new Vector3(
                Mathf.Clamp(screenPostion.x, 0 + offset, Camera.main.pixelWidth - offset),
                Mathf.Clamp(screenPostion.y, 0 + offset, Camera.main.pixelHeight - offset * 2), // Increase offset to allow the animation to be seen
                screenPostion.z
            );
        }

        var rewardIndicatorObject = Instantiate(rewardIndicatorPrefab);
        rewardIndicatorObject.transform.position = screenPostion;

        var rewardIndicator = rewardIndicatorObject.GetComponent<ScoreRewardIndicator>();
        rewardIndicator.Text = string.Format("+{0}", scoreAmount);
        rewardIndicator.FontSize = (int)Mathf.Lerp((float)comboSystem.Combo / ComboColorResolver.H_VALUE_THRESHOLD, 52, 72);
        rewardIndicator.Color = ComboColorResolver.Resolve(comboSystem.Combo);

        rewardIndicatorObject.transform.SetParent(transform, true);
    }
}
