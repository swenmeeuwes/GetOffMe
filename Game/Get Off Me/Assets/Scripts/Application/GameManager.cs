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

            if (value == GameState.GAMEOVER)
                HandleGameOver();
        }
    }
    private SaveGameModel _saveGameModel;
    public SaveGameModel SaveGame
    {
        get
        {
            if (_saveGameModel == null)
            {
                // Default save game
                var difficultyModifierDatabase = ResourceLoadService.Instance.Load<DifficultyModifierDatabase>(ResourceLoadService.DIFFICULTY_MODIFIER_DATABASE);
                _saveGameModel = new SaveGameModel()
                {
                    DifficultyModifiers = difficultyModifierDatabase.difficultyModifiers,
                    EnemyKillCount = new List<int>()
                };
                for (int i = 0; i < Enum.GetNames(typeof(EntityType)).Length; i++)
                {
                    _saveGameModel.EnemyKillCount.Add(0);
                }
                Save();
            }

            return _saveGameModel;
        }
        set
        {
            _saveGameModel = value;
        }
    }
    private GameManager()
    {
        justUnlockedVials = new Queue<VialData>();
        vialContext = ResourceLoadService.Instance.Load<VialContext>(ResourceLoadService.VIAL_CONTEXT_PATH);
        _state = GameState.MAINMENU;

        if (SceneManager.GetActiveScene().name == "Game")
            Play();

        Debug.Log("Application persistent data path: " + Application.persistentDataPath);

        // Attempt to load save game
        Load();
    }
    public void UnlockVial(VialType vialType) {
        if (!VialIsUnlocked(vialType)) {
            justUnlockedVials.Enqueue(vialContext.data.Where((vialData) => vialData.type == vialType).First());
            SaveGame.DifficultyModifiers.Where((modifier) => modifier.Type == vialType).First().Unlocked = true;
        }  
    }
    public bool VialIsUnlocked(VialType vial) {
        return SaveGame.DifficultyModifiers.Where((modifier) => modifier.Type == vial).First().Unlocked;
    }
    public void HandleHighestTimeAboveHighCombo(float time) {
        SaveGame.HighestTimeWithoutLosingHighCombo = Math.Max(time, SaveGame.HighestTimeWithoutLosingHighCombo);
    }
    public void HandleEnemiesKilledWithoutGettingHit(int enemyCount) {
        SaveGame.HighestEnemyKillCountWithoutGettingHit = Math.Max(enemyCount, SaveGame.HighestEnemyKillCountWithoutGettingHit);
    }
    private void HandleVialsStats() {
        switch (State) {
            case GameState.GAMEOVER:

                SaveGame.TotalTimeAlive += (Time.time - timeGameStarted);
                SaveGame.TotalGamesPlayed++;
                
                if (ComboSystem.Instance.completingVialRequirement) {
                    HandleHighestTimeAboveHighCombo(Time.time - ComboSystem.Instance.startTimeUnlockVial);
                }
                foreach (VialData vial in vialContext.data)
                {
                    if (UnlockConditionResolver.ConditionsAreMet(vial))
                    {
                        UnlockVial(vial.type);
                    }
                }
                Save();
                break;
            case GameState.PLAY:
                timeGameStarted = Time.time;
                break;
        }
    }
    public void GameOverNextPanel() {
        if (justUnlockedVials.Count <= 0) {
            if (UnlockedVialPanel.Instance != null) {
                UnlockedVialPanel.Instance.gameObject.SetActive(false);
            }

            if (GameOverPanel.Instance != null)
                GameOverPanel.Instance.Show();
        }else{
            if (UnlockedVialPanel.Instance != null) {
                UnlockedVialPanel.Instance.ShowUnlockedVial(justUnlockedVials.Dequeue());
            }
        }
    }
    public void Save()
    {
        try
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + SAVEGAME_FILE_NAME);
            binaryFormatter.Serialize(file, SaveGame);
            file.Close();
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
        }
    }

    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + SAVEGAME_FILE_NAME))
        {
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + SAVEGAME_FILE_NAME, FileMode.Open);
                SaveGame = (SaveGameModel)binaryFormatter.Deserialize(file);
                file.Close();

                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                return false;
            }
        }

        return false;
    }

    private void HandleGameOver()
    {
        GameOverNextPanel();
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
