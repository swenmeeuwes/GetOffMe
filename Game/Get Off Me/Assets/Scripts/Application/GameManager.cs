using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public static readonly string SAVEGAME_FILE_NAME = "savegame.sav";

    private static GameManager _instance;

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
        }
    }

    public SaveGameModel SaveGame { get; set; }

    public GameManager()
    {
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
                DifficultyModifiers = difficultyModifierDatabase.difficultyModifiers
            };
            Save();
            //
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
