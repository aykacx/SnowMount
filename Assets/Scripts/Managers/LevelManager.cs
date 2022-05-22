using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Level[] levels;
    private int _currentLevel;

    private Player player;
    private Vector3 playerDefaultPos;
    public Transform cameraTransform;
    private Vector3 cameraDefaultPos;

    public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }

    private void Start()
    {
        CurrentLevel = PlayerPrefs.GetInt("Level", 0);
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerDefaultPos = player.transform.position;
        cameraDefaultPos = cameraTransform.position;
        StartLevel();
    }

    public void StartLevel()
    {
        SetActivateLevel(true);
        player.transform.position = playerDefaultPos;
        cameraTransform.position = cameraDefaultPos;
    }

    public void StartNextLevel()
    {
        SetActivateLevel(false);
        CurrentLevel++;
        StartLevel();
        PlayerPrefs.SetInt("Level", CurrentLevel);
        PlayerPrefs.Save();
    }

    void SetActivateLevel(bool isActive)
    {
        levels[CurrentLevel % levels.Length].gameObject.SetActive(isActive);
    }
}
