using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class GameManager : MonoBehaviour, ISaveable
{   
    public static GameManager instance;

    public TextMeshProUGUI _scoreText;

    [Header("Savable var")]
    public GameObject player;
    public float _playerPosX;
    public float _playerPosY;
    public float _playerPosZ;
    public int CurrentScore;

    private List<Enemy> _enemies = new List<Enemy>();
    private HashSet<Enemy> _destroyedEnemies = new HashSet<Enemy>();
    private List<int> _destroyedEnemyIds = new List<int>();

    public List<GameObject> _doors = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadJsonData(this);
        UpdateScore();
        UpdatePlayerPos();
        UpdateDoors();
    }

    private void UpdateScore()
    {
        _scoreText.text = CurrentScore.ToString();
    }

    private void UpdatePlayerPos()
    {
        player.transform.position = new Vector3(_playerPosX, _playerPosY, _playerPosZ);
    }
    private void UpdateDoors()
    {

    }
    public void AddScore(int a_Score)
    {
        CurrentScore += a_Score;
        UpdateScore();
    }

    public void DestroyEnemy(Enemy a_Enemy)
    {
        _destroyedEnemies.Add(a_Enemy);
    }

    //Saving and loading Data
    private static void SaveJsonData(GameManager a_GameManager)
   {
        SaveData sd = new SaveData();
        a_GameManager.PopulateSaveData(sd);

        if (FileManager.WriteToFile("SaveData.dat", sd.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    private static void LoadJsonData(GameManager a_GameManager)
    {
        if (FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            a_GameManager.LoadFromSaveData(sd);
            Debug.Log("Load complete");
        }
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        a_SaveData.m_Score = CurrentScore;

        a_SaveData.m_playerPosX = player.transform.position.x;
        a_SaveData.m_playerPosY = player.transform.position.y;
        a_SaveData.m_playerPosZ = player.transform.position.z;

        a_SaveData.doorList = _doors;

        foreach (Enemy enemy in _enemies)
        {
            enemy.PopulateSaveData(a_SaveData);
        }
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        CurrentScore = a_SaveData.m_Score;

        _playerPosX = a_SaveData.m_playerPosX;
        _playerPosY = a_SaveData.m_playerPosY;
        _playerPosZ = a_SaveData.m_playerPosZ;

        _doors = a_SaveData.doorList;

        foreach (Enemy enemy in _enemies)
        {
            enemy.LoadFromSaveData(a_SaveData);
        }

        foreach (int enemyId in _destroyedEnemyIds)
        {
            SaveData.EnemyData enemyData = new SaveData.EnemyData();
            enemyData.m_Health = 0;
            enemyData.m_Id = enemyId;
            a_SaveData.m_EnemyData.Add(enemyData);
        }
    }
    private void OnApplicationQuit()
    {
        _enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        _doors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Door"));
        SaveJsonData(this);
    }
}