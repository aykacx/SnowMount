using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Start,
    Pause,
    End
}
public class GameManager : MonoBehaviour
{
    LevelManager levelManager;
    private GameState _currentGameState;

    public Text levelText;
    public const string LEVEL_STRING = "Level";

    public GameState CurrentGameState { get => _currentGameState; set => _currentGameState = value; }

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        CurrentGameState = GameState.Pause;
        UpdateLevelText(levelManager.CurrentLevel);
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = LEVEL_STRING + (level + 1);
    }
    public void StartGame()
    {
        CurrentGameState = GameState.Start;
        UpdateLevelText(levelManager.CurrentLevel);
        levelManager.StartLevel();
    }
    public void StartNextGame()
    {
        levelManager.StartNextLevel();
        UpdateLevelText(levelManager.CurrentLevel);
    }

    public void EndGame()
    {
        CurrentGameState = GameState.End;
    }
}
