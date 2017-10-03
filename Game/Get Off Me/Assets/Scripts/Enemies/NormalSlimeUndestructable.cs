using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSlimeUndestructable : NormalSlimeEnemy {

	// Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        entityType = EntityType.SLIME_NORMAL_TUTORIAL;
        IgnoreTap = true;
        Draggable = false;
    }
}
