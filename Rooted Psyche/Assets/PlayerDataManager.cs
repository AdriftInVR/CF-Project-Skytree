using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerDataManager : MonoBehaviour
{
    public Transform playerTransform;
    public int score;

    public void SaveGame()
    {
        PlayerData playerData = new PlayerData();
        playerData.position = new float[] { playerTransform.position.x, playerTransform.position.y, playerTransform.position.z };

        string json = JsonUtility.ToJson(playerData);
        string path = Application.persistentDataPath + "/playerData.json";
        System.IO.File.WriteAllText(path, json);
    
    }

  
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);

            // update player position
            playerTransform.position = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);
            Vector3 loadedPosition = new Vector3(loadedData.position[0], loadedData.position[1], loadedData.position[2]);

            // load all values from the file playerData.json
            playerTransform.position = loadedPosition;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
        }
    }
}
