﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSpawner : MonoBehaviour, ISpawner
{
    // Exposed inspector fields
    [SerializeField]
    private bool enabledByDefault;
    [SerializeField][Tooltip("The time it takes in seconds for the first object to spawn.")]
    protected float initialDelay;
    [SerializeField][Tooltip("The time in seconds in between spawns.")]
    protected float interval;
    // ---

    private readonly string SPAWN_METHOD_NAME = "Spawn";
    private readonly string SPAWN_LIST_NAME = "spawns";

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
            if (!Enabled && newState)
                InvokeRepeating(SPAWN_METHOD_NAME, initialDelay, interval);
            else if (Enabled && !newState)
                CancelInvoke(SPAWN_METHOD_NAME);

            _enabled = enabled;
        }
    }

    private Transform SpawnListTransform
    {
        get
        {
            return transform.Find(SPAWN_LIST_NAME);
        }
    }

    public virtual void Start()
    {
        Enabled = enabledByDefault;

        // Create a empty game object to keep the spawns
        var spawnsList = new GameObject(SPAWN_LIST_NAME);
        spawnsList.transform.parent = transform;
    }

    public abstract void Spawn();

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
    protected GameObject CreateSpawn(GameObject objectToSpawn)
    {
        var spawned = Instantiate(objectToSpawn);
        spawned.transform.parent = SpawnListTransform;

        return spawned;
    }
}