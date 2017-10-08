using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEntitySpawner : AbstractSpawner {
    // Exposed inspector fields
    [SerializeField]
    private AnimationCurve speedAdditionCurve;
    // ---

    // Should this not be put in a "WaveSpawner"?
    enum SpawnState
    {
        WAVE,
        REST
    }

    public float wavePeriod = 12;
    public float restPeriod = 5;
    public float restPeriodAdditionalInterval = 1.0f;

    SpawnState spawnState = SpawnState.WAVE;

    public override void Spawn()
    {
        if (Enabled)
            Invoke(SPAWN_METHOD_NAME, spawnRateCurve.Evaluate(counter) + (spawnState == SpawnState.REST ? restPeriodAdditionalInterval : 0));
    }

    public override GameObject CreateSpawn(GameObject objectToSpawn)
    {
        var spawned = base.CreateSpawn(objectToSpawn);

        // Speed addition
        var speedAddition = speedAdditionCurve.Evaluate(counter);

        var entity = spawned.GetComponent<AbstractEntity>();
        entity.model.speed += speedAddition;

        return spawned;
    }

    public void SetWave()
    {
        spawnState = SpawnState.WAVE;
        Invoke("SetRest", wavePeriod);
    }

    public void SetRest()
    {
        spawnState = SpawnState.REST;
        Invoke("SetWave", restPeriod);
    }
}
