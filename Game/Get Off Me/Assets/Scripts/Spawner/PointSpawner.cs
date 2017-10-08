using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawner : AbstractSpawner {
    [SerializeField]
    private GameObject[] spawnList;

    public override void Spawn()
    {
        var spawn = base.CreateSpawn(spawnList.RandomItem());
        spawn.transform.position = transform.position;

        base.Spawn();
    }
}
