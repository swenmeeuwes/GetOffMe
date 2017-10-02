using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public static readonly string SAVEGAME_FILE_NAME = "savegame.sav";


    private static GameManager _instance;
    private float timeGameStarted;
    private VialContext vialContext;
    private Queue<VialData> justUnlockedVials;
    private GameObject gameOverPanel;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    private GameState _state;
    public GameState State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
            HandleVialsStats();
        }
    }

    public SaveGameModel SaveGame { get; set; }
    private GameManager()
    {
        justUnlockedVials = new Queue<VialData>();
        vialContext = ResourceLoadService.Instance.Load<VialContext>(ResourceLoadService.VIAL_CONTEXT_PATH);
        _state = GameState.MAINMENU;

        if (SceneManager.GetActiveScene().name == "Game")
            Play();

        Debug.Log("Application persistent data path: " + Application.persistentDataPath);

        if (!Load())
        {
            // No save game exists, create a new one!
            // TEMP
            var difficultyModifierDatabase = Resources.Load<DifficultyModifierDatabase>("Config/DifficultyModifierDatabase");
            SaveGame = new SaveGameModel()
            {
                DifficultyModifiers = difficultyModifierDatabase.difficultyModifiers,
                EnemyKillCount = new List<int>()
            };
            for (int i = 0; i < Enum.GetNames(typeof(EntityType)).Length; i++) {
                SaveGame.EnemyKillCount.Add(0);
            }
            Save();
            //
        }
    }
    public void UnlockVial(VialType vialType) {
        if (!vialIsUnlocked(vialType)) {
            justUnlockedVials.Enqueue(vialContext.data.Where((vialData) => vialData.type == vialType).First());
            SaveGame.DifficultyModifiers.Where((modifier) => modifier.Type == vialType).First().Unlocked = true;
        }  
    }
    public bool vialIsUnlocked(VialType vial) {
        return SaveGame.DifficultyModifiers.Where((modifier) => modifier.Type == vial).First().Unlocked;
    }
    public void HandleHighestTimeAboveHighCombo(float time) {
        SaveGame.HighestTimeWithoutLosingHighCombo = Math.Max(time, SaveGame.HighestTimeWithoutLosingHighCombo);
        if (SaveGame.HighestTimeWithoutLosingHighCombo > 100.0f) {
            UnlockVial(VialType.SPAWN_VIAL);
        }
    }
    public void HandleEnemiesKilledWithoutGettingHit(int enemyCount) {
        SaveGame.HighestEnemyKillCountWithoutGettingHit = Math.Max(enemyCount, SaveGame.HighestEnemyKillCountWithoutGettingHit);
        if(enemyCount > 5) {
            UnlockVial(VialType.SPEED_VIAL);
        }
    }
    private void HandleVialsStats() {
        switch (State) {
            case GameState.GAMEOVER:

                SaveGame.TotalTimeAlive += (Time.time - timeGameStarted);
                SaveGame.TotalGamesPlayed++;
                if (SaveGame.TotalTimeAlive > 3600) {
                    UnlockVial(VialType.SPAWN_VIAL);
                }

                if (SaveGame.EnemyKillCount[(int)EntityType.SLIME_HELMET] >= 1) {
                    UnlockVial(VialType.HELMET_VIAL);
                }

                if (SaveGame.EnemyKillCount[(int)EntityType.SLIME_ROGUE] >= 400) {
                    UnlockVial(VialType.ROGUE_VIAL);
                }

                if (SaveGame.EnemyKillCount[(int)EntityType.SLIME_WIZARD] >= 300) {
                    UnlockVial(VialType.WIZARD_VIAL);
                }
                if (ComboSystem.Instance.completingVialRequirement) {
                    HandleHighestTimeAboveHighCombo(Time.time - ComboSystem.Instance.startTimeUnlockVial);
                }
                Save();
                break;
            case GameState.PLAY:
                timeGameStarted = Time.time;
                break;
        }
    }
    public void GameOverSequence(GameObject gameOverPanel) {
        this.gameOverPanel = gameOverPanel;
        GameOverNextPanel();
    }
    public void GameOverNextPanel() {
        if (justUnlockedVials.Count <= 0) {
            UnlockedVialPanel.Instance.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
        }else{
            if (UnlockedVialPanel.Instance != null) {
                UnlockedVialPanel.Instance.ShowUnlockedVial(justUnlockedVials.Dequeue());
            }
        }
    }
    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + SAVEGAME_FILE_NAME);
        binaryFormatter.Serialize(file, SaveGame);
        file.Close();
    }

    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + SAVEGAME_FILE_NAME))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + SAVEGAME_FILE_NAME, FileMode.Open);
            SaveGame = (SaveGameModel)binaryFormatter.Deserialize(file);
            file.Close();

            return true;
        }

        return false;
    }

    public void Play()
    {
        _state = GameState.PLAY;
        Time.timeScale = 1;
    }
    public void Pause()
    {
        Time.timeScale = 0;
        _state = GameState.PAUSE;
    }
    public void Resume()
    {
        Time.timeScale = 1;
        _state = GameState.PLAY;
    }
    public void PrepareMainMenu()
    {
        Time.timeScale = 1;
        _state = GameState.MAINMENU;
    }
}
