using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayer : MonoBehaviour {
    [SerializeField]
    private OffScreenSpawner spawner;
    [SerializeField]
    private GameObject[] tutorialEncounters;
    [SerializeField]
    private Sprite tapDialog;
    [SerializeField]
    private Sprite swipeDialog;
    [SerializeField]
    private GameObject instructionCanvasPrefab;
    [SerializeField]
    private Text tutorialTextField;
    [SerializeField]
    private string[] tutorialTextSequence;

    private Animation textAnimation;

    private GameObject player;

    private int tutorialSequenceIndex;
    private int encounterIndex;

    private void Start()
    {
        textAnimation = tutorialTextField.GetComponent<Animation>();

        player = GameObject.FindGameObjectWithTag("Player");

        tutorialSequenceIndex = 0;
        encounterIndex = 0;

        spawner.Enabled = false; // Halt spawning

        tutorialTextField.text = "";

        Next();
    }

    private void Next()
    {
        if (tutorialSequenceIndex < tutorialTextSequence.Length)
        {
            HandleText();
            tutorialSequenceIndex++;
        }
        else if (encounterIndex < tutorialEncounters.Length)
        {
            HandleEncounter();
            encounterIndex++;
        }
        else
        {
            // Tutorial is finished
            spawner.Enabled = true;
        }
    }

    private void HandleText()
    {
        // Show text
        tutorialTextField.text = tutorialTextSequence[tutorialSequenceIndex];
        textAnimation.PlayQueued("TextFadeInAnimation", QueueMode.PlayNow);
        StartCoroutine(AnimationUtil.OnAnimationFinished(textAnimation, () => {
            Invoke("HideText", 1.5f);
        }));
    }

    private void HideText()
    {
        textAnimation.PlayQueued("TextFadeOutAnimation", QueueMode.PlayNow);

        StartCoroutine(AnimationUtil.OnAnimationFinished(textAnimation, () => {
            Invoke("Next", 0.5f);
        }));
    }

    private void HandleEncounter()
    {
        var nextEntity = tutorialEncounters[encounterIndex].GetComponent<AbstractEntity>(); // ASSUMPTION: Encounter is an entity!
        var randomPosition = spawner.GetRandomSpawnPoint();

        //var sprite = new GameObject("DialogBalloon");
        //var spriteRenderer = sprite.AddComponent<SpriteRenderer>();
        //sprite.transform.position = randomPosition + new Vector2(0, 1);

        var instructionCanvas = Instantiate(instructionCanvasPrefab);

        var spawned = spawner.CreateSpawn(nextEntity.gameObject);
        var spawnedEntity = spawned.GetComponent<AbstractEntity>();
        spawned.transform.position = randomPosition;

        spawnedEntity.model.health = 1;
        //spawnedEntity.model.speed = 1f;
        spawnedEntity.model.varianceInSpeed = 0f;

        //sprite.transform.parent = spawned.transform;
        //instructionCanvas.transform.parent = spawned.transform;
        //instructionCanvas.GetComponent<RectTransform>().localPosition = new Vector3(0, -0.6f, 0);

        instructionCanvas.transform.SetParent(spawned.transform, false);


        if (spawnedEntity is HelmetSlimeEnemy)
        {
            //spriteRenderer.sprite = tapDialog;
            //spawnedEntity.AddEventListener("tapped", (e) => spriteRenderer.sprite = swipeDialog, true);

            instructionCanvas.GetComponentInChildren<Text>().text = "Tap";
        }
        else
        {
            //spriteRenderer.sprite = swipeDialog;
            instructionCanvas.GetComponentInChildren<Text>().text = "Swipe";
        }

        spawnedEntity.AddEventListener("dying", (e) => Next(), true);
    }
}
