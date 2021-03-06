﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawner : MonoBehaviour, ISpawner
{
    // Exposed inspector fields
    [SerializeField]
    private bool enabledByDefault;
    [SerializeField]
	public AnimationCurve spawnRateCurve;
    // ---

    protected readonly string SPAWN_METHOD_NAME = "Spawn";
    protected readonly string SPAWN_LIST_NAME = "spawns";

    protected float counter = 0;
    private bool _enabled;

    public bool Enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            var newState = value;
            if (!_enabled && newState)
                Invoke(SPAWN_METHOD_NAME, spawnRateCurve.Evaluate(counter));
            else if (Enabled && !newState)
                CancelInvoke(SPAWN_METHOD_NAME);

            _enabled = newState;
        }
    }

    private Transform SpawnListTransform
    {
        get
        {
            return transform.Find(SPAWN_LIST_NAME);
        }
    }

    protected virtual void Awake()
    {
        Enabled = enabledByDefault;
    }

    public virtual void Start()
    {
        // Create a empty game object to keep the spawns
        var spawnsList = new GameObject(SPAWN_LIST_NAME);
        spawnsList.transform.parent = transform;
    }

    protected virtual void Update()
    {
        if(Enabled)
            counter += Time.deltaTime;
    }

    public virtual void Spawn()
    {
        if(Enabled)
            Invoke(SPAWN_METHOD_NAME, spawnRateCurve.Evaluate(counter));
    }

    /// <summary>
    /// Gets an array of alive spawned game objects
    /// </summary>
    /// <returns>An array of spawned game objects that are still alive</returns>
    public GameObject[] GetAllSpawns()
    {
        var spawnCount = SpawnListTransform.childCount;

        var spawns = new GameObject[spawnCount];
        for (int i = 0; i < spawnCount; i++)
        {
            spawns[i] = SpawnListTransform.GetChild(i).gameObject;
        }
        return spawns;
    }

    /// <summary>
    /// Destroys all alive spawns
    /// </summary>
    /// <returns>Whenever 1 or more spawn were destroyed</returns>
    public bool DestroyAllSpawns()
    {
        var spawns = GetAllSpawns();
        var amountOfSpawns = spawns.Length;
        for (int i = 0; i < spawns.Length; i++)
        {
            Destroy(spawns[i].gameObject);
        }

        return amountOfSpawns > 0;
    }

    /// <summary>
    /// Instantiates and registers the provided prefab
    /// </summary>
    /// <param name="objectToSpawn">The prefab to spawn</param>
    /// <returns>The spawned game object</returns>
    public virtual GameObject CreateSpawn(GameObject objectToSpawn)
    {
        var spawned = Instantiate(objectToSpawn);
        spawned.transform.parent = SpawnListTransform;

        return spawned;
    }
}
