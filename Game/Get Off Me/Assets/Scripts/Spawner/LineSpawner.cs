using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpawner : AbstractSpawner {
    [SerializeField]
    private GameObject[] spawnList;
    [SerializeField]
    private Vector3 p1;
    [SerializeField]
    private Vector3 p2;

    public override void Spawn()
    {
        var spawn = base.CreateSpawn(spawnList.RandomItem());
        spawn.transform.position = Vector3.Lerp(p1, p2, Random.value);

        base.Spawn();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(p1, p2);
    }
}
