using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSlimeUndestructable : NormalSlimeEnemy {

    protected override void Awake()
    {
        base.Awake();
        entityType = EntityType.SLIME_NORMAL_TUTORIAL;
        IgnoreTap = true;
        Draggable = false;
        ShowParticles = false;
    }
}
