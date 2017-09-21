using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayer : MonoBehaviour {
    [SerializeField]
    private OffScreenSpawner spawner;
    [SerializeField]
    private GameObject[] tutorialEncounters;
    //[SerializeField]
    //private Sprite tapDialog;
    //[SerializeField]
    //private Sprite swipeDialog;
    [SerializeField]
    private GameObject instructionCanvasPrefab;
    [SerializeField]
    private GameObject tutorialCanvas;
    [SerializeField]
    private Text tutorialTextField;
    [SerializeField]
    private string[] tutorialTextSequence;

    //public TutorialSequence tutorialSequence;
    public List<TutorialItem> tutorialSequence = new List<TutorialItem>();

    private Animation textAnimation;
    private Camera tutorialCamera;

    private GameObject player;

    private int tutorialSequenceIndex;
    private int encounterIndex;

    private void Start()
    {
        textAnimation = tutorialTextField.GetComponent<Animation>();
        tutorialCamera = GetComponent<Camera>();

        player = GameObject.FindGameObjectWithTag("Player");

        tutorialCanvas.SetActive(false);
        tutorialCamera.enabled = false;

        tutorialSequenceIndex = 0;
        encounterIndex = 0;

        spawner.Enabled = false; // Halt spawning

        tutorialTextField.text = "";

        if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1)
            Next();
        else
            Finish();
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
            Finish();
        }
    }

    private void Finish()
    {
        spawner.Enabled = true;
        spawner.SetWave();
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

        var instructionCanvas = Instantiate(instructionCanvasPrefab);

        var spawned = spawner.CreateSpawn(nextEntity.gameObject);
        var spawnedEntity = spawned.GetComponent<AbstractEntity>();
        spawned.transform.position = randomPosition;

        spawnedEntity.model.health = 1;
        spawnedEntity.model.varianceInSpeed = 0f;

        instructionCanvas.transform.SetParent(spawned.transform, false);


        if (spawnedEntity is HelmetSlimeEnemy)
        {
            instructionCanvas.GetComponentInChildren<Text>().text = "Tap";
            spawnedEntity.AddEventListener("tapped", (e) => instructionCanvas.GetComponentInChildren<Text>().text = "Swipe", true);
        }
        else
        {
            instructionCanvas.GetComponentInChildren<Text>().text = "Swipe";
        }

        spawnedEntity.AddEventListener("dying", (e) => Next(), true);
    }
}
