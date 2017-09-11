using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicSlimeAlly : SeekingEntity {
    public override void OnPlayerHit(Player player)
    {
        player.Heal(1);
        base.OnPlayerHit(player);
    }
}
