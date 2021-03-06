﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Spawns GameObject X out of the Main Camera's sight
/// GameObjects are spawned in a radius around the camera's position
/// </summary>
public class OffScreenSpawner : AbstractEntitySpawner
{
    // Exposed inspector fields
    [SerializeField]
    private Camera orthographicCamera;
    [SerializeField]
    private float spawnDistanceOffset = 0.0f;
    [SerializeField]
    private int amountOfSpawnPoints = 180;
    [SerializeField]
    private Vector2[] degreeBlacklist;
    //[SerializeField][Tooltip("When the speed will cap (in seconds)")]
    //private float speedAdditionTimeCap;
    // ---


    [HideInInspector]
    public List<GamePhase> gamePhases;
    private GamePhase currentPhase;

    private bool spawnPointsInitialized = false;
	private List<IVial> activeVials;

    private float OffsettedScreenDiagonal
    {
        get
        {
            var cameraHeight = orthographicCamera.orthographicSize * 2f;
            var cameraWidth = cameraHeight * orthographicCamera.aspect;

            var screenDiagonal = (float)Math.Sqrt(Math.Pow(cameraHeight, 2) + Math.Pow(cameraWidth, 2));


            return screenDiagonal + spawnDistanceOffset;
        }
    }

    private Vector2[] spawnPoints;


    public override void Start()
    {
        base.Start();
		activeVials = VialManager.Instance.GetActiveVials();
        gamePhases = loadGamePhasesFromFile();
        currentPhase = gamePhases[0];
        if (orthographicCamera == null)
            orthographicCamera = Camera.main;

        InitializeSpawnPoints(amountOfSpawnPoints);
    }
    protected override void Update()
    {
        base.Update();

        if (Enabled)
        {
            // OPTIMAZATION: setTimeout(function(){ goToNextPhase }, nextPhase.time - currentPhase.time);
            for (int i = 0; i < gamePhases.Count; i++)
            {
                if (counter > gamePhases[i].time)
                {
                    currentPhase = gamePhases[i];
                    break;
                }
            }
        }
    }

    public override void Spawn()
    {
        if (GameManager.Instance.State != GameState.PLAY)
            return;

        var randomSpawnPosition = GetRandomSpawnPoint();

        GameObject randomEntity = base.CreateSpawn(GetRandomEntityFromSpawnList(currentPhase));
        randomEntity.transform.position = randomSpawnPosition;
        var entity = randomEntity.GetComponent<AbstractEntity>();

        if (GameManager.Instance.State == GameState.PLAY) {
            for (int i = 0; i < activeVials.Count; i++) {
			    entity.Accept (activeVials [i]);
		    }
        }
        
        base.Spawn();
    }

    /// <summary>
    /// Initializes all spawn points on the spawn circle
    /// </summary>
    /// <param name="precision">Amount of spawn amounts</param>
    private void InitializeSpawnPoints(int precision)
    {
        if (spawnPointsInitialized)
            return;

        spawnPointsInitialized = true;

        if (precision < 0)
        {
            spawnPoints = new Vector2[0];
            return;
        }
            
        var spawnCircleRadius = OffsettedScreenDiagonal / 2f;

        var possibleSpawnPoints = new List<Vector2>();
        for (int i = 0; i < precision; i++)
        {
            var degree = ((float)i / (float)precision) * 360f;
            var radianAngle = degree * Mathf.Deg2Rad;

            if (DegreeIsBlackListed(degree))
                continue;

            var x = (float)(orthographicCamera.transform.position.x + spawnCircleRadius * Math.Cos(radianAngle));
            var y = (float)(orthographicCamera.transform.position.y + spawnCircleRadius * Math.Sin(radianAngle));

            possibleSpawnPoints.Add(new Vector2(x, y));
        }

        spawnPoints = possibleSpawnPoints.ToArray();
    }

    public Vector2 GetRandomSpawnPoint()
    {
        if (spawnPoints == null)
            InitializeSpawnPoints(amountOfSpawnPoints);

        var randomSpawnPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)];
        return randomSpawnPosition;
    }

    private void OnDrawGizmos()
    {
        // While NOT in play-mode
        if (orthographicCamera != null)
            InitializeSpawnPoints(amountOfSpawnPoints);
        // ---

        if (orthographicCamera != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(orthographicCamera.transform.position, OffsettedScreenDiagonal / 2f);
        }

        if (spawnPoints != null)
        {            
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(spawnPoints[i], Vector3.one * 0.2f);                
            }
        }
    }

    private bool DegreeIsBlackListed(float degree)
    {
        if(degreeBlacklist == null)
            return false;

        for (int i = 0; i < degreeBlacklist.Length; i++)
        {
            var item = degreeBlacklist[i];
            if (item.x <= degree && item.y >= degree)
                return true;
        }

        return false;
    }

    private GameObject GetRandomEntityFromSpawnList(GamePhase phase)
    {

        float totalWeight = 0;
        for (int i = 0; i < phase.weights.Count; i++) {
            totalWeight += phase.weights[i];
        }

        float randomNumber = UnityEngine.Random.Range(0.0f, totalWeight);

        float cumulativeCounter = 0;
        for (int i = 0; i < phase.weights.Count; i++)
        {
            cumulativeCounter += phase.weights[i];
            if (randomNumber <= cumulativeCounter)
            {
                return phase.objectKeys[i];
            }
        }

        Debug.LogWarning("Error While spawning enemy");
        throw new Exception("Error While spawning enemy");
    }
    public List<GamePhase> loadGamePhasesFromFile() // TEMPORARY, MOVE THIS.
    {
        List<GamePhase> gamePhases = new List<GamePhase>();
        foreach (GamePhase g in Resources.LoadAll("Phases", typeof(GamePhase)))
        {
            gamePhases.Add(g);
        }

        gamePhases.Sort((g1, g2) => g2.time.CompareTo(g1.time));
        return gamePhases;
    }
}
