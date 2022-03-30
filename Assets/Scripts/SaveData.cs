using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int m_Score;
    public float m_playerPosX;
    public float m_playerPosY;
    public float m_playerPosZ;

    public List<GameObject> doorList = new List<GameObject>();

    [System.Serializable]
    public struct EnemyData
    {
        public int m_Id;
        public int m_Health;
    }

    public List<EnemyData> m_EnemyData = new List<EnemyData>();
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}
