using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour // peut etre le mettre dans le gameManager
{
    public GameObject player;

    void SavePlayerData(int dollCount, Vector3 playerPos, Dictionary<int, bool> dictionaryDoors) //on récup les data du player
    {
        PlayerPrefs.SetInt("doll", dollCount);
        PlayerPrefs.SetFloat("playerX", playerPos.x);
        PlayerPrefs.SetFloat("playerY", playerPos.y);
        PlayerPrefs.SetFloat("playerZ", playerPos.z);

        foreach(var item in dictionaryDoors)
        {
            PlayerPrefs.SetString(item.Key.ToString(), item.Value.ToString());
        }

        PlayerPrefs.Save();
    }

    void LoadSave()
    {
        var doll = PlayerPrefs.GetInt("doll");
        var posX = PlayerPrefs.GetFloat("playerX");
        var posY = PlayerPrefs.GetFloat("playerY");
        var posZ = PlayerPrefs.GetFloat("playerZ");

        //player.doll = doll;
        player.transform.position = new Vector3(posX, posY, posZ);
    }
}


