using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayer : MonoBehaviour {
    [SerializeField]
    private OffScreenSpawner spawner;
    [SerializeField]
    private GameObject instructionCanvasPrefab;
    [SerializeField]
    private GameObject tutorialCanvas;
    [SerializeField]
    private Text tutorialTextField;

    public List<TutorialSequenceItem> tutorialSequence = new List<TutorialSequenceItem>();

    private Animation textAnimation;
    private Camera tutorialCamera;

    private Player player;

    private int tutorialSequenceIndex;
    private int encounterIndex;

    private void Start()
    {
        textAnimation = tutorialTextField.GetComponent<Animation>();
        tutorialCamera = GetComponent<Camera>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        tutorialCanvas.SetActive(false);
        tutorialCamera.enabled = false;

        tutorialSequenceIndex = 0;
        encounterIndex = 0;

        spawner.Enabled = false; // Halt spawning

        tutorialTextField.text = "";

        if (PlayerPrefs.GetInt("ShowTutorial", 1) == 1)
        {
            player.Damage(1);
            Next();
        }
        else
        {
            Finish();
        }
    }

    private void Next()
    {
        if (tutorialSequenceIndex < tutorialSequence.Count)
        {
            var sequenceItem = tutorialSequence[tutorialSequenceIndex];
            HandleSequenceItem(sequenceItem);

            tutorialSequenceIndex++;
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

    private void HandleSequenceItem(TutorialSequenceItem sequenceItem)
    {
        switch (sequenceItem.type)
        {
            case TutorialSequenceItemType.TEXT:
                HandleText(sequenceItem.textContent, sequenceItem.textDuration);
                break;
            case TutorialSequenceItemType.SPAWN:
                HandleSpawn(sequenceItem.spawnPrefab);
                break;
        }
    }

    private void HandleText(string text, float duration)
    {
        // Show text
        tutorialTextField.text = text;
        textAnimation.PlayQueued("TextFadeInAnimation", QueueMode.PlayNow);
        StartCoroutine(AnimationUtil.OnAnimationFinished(textAnimation, () => {
            Invoke("HideText", duration);
        }));
    }

    private void HideText()
    {
        textAnimation.PlayQueued("TextFadeOutAnimation", QueueMode.PlayNow);

        StartCoroutine(AnimationUtil.OnAnimationFinished(textAnimation, () => {
            Invoke("Next", 0.1f);
        }));
    }

    private void HandleSpawn(GameObject spawnPrefab)
    {
        var nextEntity = spawnPrefab.GetComponent<AbstractEntity>(); // ASSUMPTION: Encounter is an entity!
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

        if(spawnedEntity is MedicSlimeAlly)
            instructionCanvas.GetComponentInChildren<Text>().text = "";

        spawnedEntity.AddEventListener("dying", (e) => Next(), true);
    }
}
