using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour {
    [SerializeField]
    private OffScreenSpawner spawner;
    [SerializeField]
    private GameObject[] tutorialEncounters;
    [SerializeField]
    private Sprite tapDialog;
    [SerializeField]
    private Sprite swipeDialog;

    private int encounterIndex;

    private void Start()
    {
        encounterIndex = 0;

        spawner.Enabled = false; // Halt spawning

        Next();
    }

    private void Next()
    {
        if(encounterIndex > tutorialEncounters.Length - 1)
        {
            spawner.Enabled = true;
            return;
        }

        var nextEntity = tutorialEncounters[encounterIndex].GetComponent<AbstractEntity>(); // ASSUMPTION: Encounter is an entity!
        var randomPosition = spawner.GetRandomSpawnPoint();

        var sprite = new GameObject("DialogBalloon");
        var spriteRenderer = sprite.AddComponent<SpriteRenderer>();
        sprite.transform.position = randomPosition + new Vector2(0, 1);

        var spawned = spawner.CreateSpawn(nextEntity.gameObject);
        var spawnedEntity = spawned.GetComponent<AbstractEntity>();
        spawned.transform.position = randomPosition;

        spawnedEntity.model.health = 1;
        //spawnedEntity.model.speed = 1f;
        spawnedEntity.model.varianceInSpeed = 0f;

        sprite.transform.parent = spawned.transform;

        if (spawnedEntity is HelmetSlimeEnemy)
        {
            spriteRenderer.sprite = tapDialog;
            spawnedEntity.AddEventListener("tapped", (e) => spriteRenderer.sprite = swipeDialog, true);
        }
        else
        {
            spriteRenderer.sprite = swipeDialog;
        }
            
        spawnedEntity.AddEventListener("dying", (e) => Next(), true);

        encounterIndex++;
    }
}
