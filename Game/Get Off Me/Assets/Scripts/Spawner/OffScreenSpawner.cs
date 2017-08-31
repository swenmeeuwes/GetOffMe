using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns GameObject X out of the Main Camera's sight
/// GameObjects are spawned in a radius around the camera's position
/// </summary>
public class OffScreenSpawner : AbstractSpawner
{
    // Exposed inspector fields
    [SerializeField]
    private GameObject objectToSpawn;
    [SerializeField]
    private Camera orthographicCamera;
    [SerializeField]
    private float spawnDistanceOffset = 0.0f;
    [SerializeField]
    private int amountOfSpawnPoints = 180;
    [SerializeField]
    private Vector2[] degreeBlacklist;
    // ---

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

        if (orthographicCamera == null)
            orthographicCamera = Camera.main;

        InitializeSpawnPoints(amountOfSpawnPoints);
    }

    public override void Spawn()
    {
        var randomSpawnPosition = spawnPoints[UnityEngine.Random.Range(0, amountOfSpawnPoints - 1)];

        var spawned = base.CreateSpawn(objectToSpawn);
        spawned.transform.position = randomSpawnPosition;
    }

    /// <summary>
    /// Initializes all spawn points on the spawn circle
    /// </summary>
    /// <param name="precision">Amount of spawn amounts</param>
    private void InitializeSpawnPoints(int precision)
    {
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
}
