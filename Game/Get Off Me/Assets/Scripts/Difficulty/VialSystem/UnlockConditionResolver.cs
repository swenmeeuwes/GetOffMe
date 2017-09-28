using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UnlockConditionResolver {
    public static bool ConditionsAreMet(VialData vialData)
    {
        var saveGame = GameManager.Instance.SaveGame;
        switch (vialData.unlockConditionType)
        {
            case UnlockConditions.KILL_NORMALS:
                var normalSlimeKills = saveGame.EnemyKillCount[(int)EntityType.SLIME_NORMAL];
                return normalSlimeKills >= vialData.unlockConditionValue;
            case UnlockConditions.KILL_ROGUES:
                var rogueSlimeKills = saveGame.EnemyKillCount[(int)EntityType.SLIME_ROGUE];
                return rogueSlimeKills >= vialData.unlockConditionValue;
            case UnlockConditions.KILL_TANKS:
                var tankSlimeKills = saveGame.EnemyKillCount[(int)EntityType.SLIME_HELMET];
                return tankSlimeKills >= vialData.unlockConditionValue;
            case UnlockConditions.KILL_WIZARDS:
                var wizardSlimeKills = saveGame.EnemyKillCount[(int)EntityType.SLIME_WIZARD];
                return wizardSlimeKills >= vialData.unlockConditionValue;
            case UnlockConditions.MAINTAIN_HIGH_COMBO_FOR_X_SECONDS:
                return vialData.unlockConditionValue >= saveGame.HighestTimeWithoutLosingHighCombo;
            case UnlockConditions.KILL_WITHOUT_GETTING_HIT:
                return vialData.unlockConditionValue >= saveGame.HighestEnemyKillCountWithoutGettingHit;
            case UnlockConditions.SURVIVE_OVER_MULTIPLE_GAMES:
                return vialData.unlockConditionValue >= saveGame.TotalTimeAlive;
        }

        throw new NotImplementedException(string.Format("Unlock condition {0} is not yet implemented", vialData.unlockConditionType.ToString()));
    }

    /// <summary>
    /// Get the progression on an unlock condition
    /// </summary>
    /// <param name="unlockCondition">The unlock condition in dispute</param>
    /// <returns>A value from 0 to 1 representing the progression</returns>
    public static float GetProgression(VialData vialData)
    {
        var saveGame = GameManager.Instance.SaveGame;
        
        switch (vialData.unlockConditionType)
        {
            case UnlockConditions.KILL_NORMALS:
                var normalSlimeKills = saveGame.EnemyKillCount[(int)EntityType.SLIME_NORMAL];
                return Mathf.Clamp01(normalSlimeKills / vialData.unlockConditionValue);
            case UnlockConditions.KILL_ROGUES:
                var rogueSlimeKills = saveGame.EnemyKillCount[(int)EntityType.SLIME_ROGUE];
                return Mathf.Clamp01(rogueSlimeKills / vialData.unlockConditionValue);
            case UnlockConditions.KILL_TANKS:
                var tankSlimeKills = saveGame.EnemyKillCount[(int)EntityType.SLIME_HELMET];
                return Mathf.Clamp01(tankSlimeKills / vialData.unlockConditionValue);
            case UnlockConditions.KILL_WIZARDS:
                var wizardSlimeKills = saveGame.EnemyKillCount[(int)EntityType.SLIME_WIZARD];
                return Mathf.Clamp01(wizardSlimeKills / vialData.unlockConditionValue);
            case UnlockConditions.MAINTAIN_HIGH_COMBO_FOR_X_SECONDS:
                return Mathf.Clamp01(saveGame.HighestTimeWithoutLosingHighCombo / vialData.unlockConditionValue);
            case UnlockConditions.KILL_WITHOUT_GETTING_HIT:
                return Mathf.Clamp01(saveGame.HighestEnemyKillCountWithoutGettingHit / vialData.unlockConditionValue);
            case UnlockConditions.SURVIVE_OVER_MULTIPLE_GAMES:
                return Mathf.Clamp01(saveGame.TotalTimeAlive / vialData.unlockConditionValue);
        }

        throw new NotImplementedException(string.Format("Unlock condition {0} is not yet implemented", vialData.unlockConditionType.ToString()));
    }
}
