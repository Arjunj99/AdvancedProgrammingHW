using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public PlayerData data = new PlayerData();
    public PlayerMovement playerMovement;
    public string file;
     
    public void Start()
    {
        Save();
        Load();
        //Debug.Log($"{Application.persistentDataPath}/{file}");
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(FormatData(playerMovement));
        WriteToFile(file, json);
    }

    public void Load()
    {
        data = new PlayerData();
        string json = ReadFromFile(file);
        JsonUtility.FromJsonOverwrite(json, data);
        Debug.Log(data);
        ApplySaveData(data);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogError("File not found.");
        }
        return "";
    }

    private string GetFilePath(string fileName)
    {
        return $"{Application.persistentDataPath}/{fileName}";
    }

    private PlayerData FormatData(PlayerMovement playerMovement)
    {
        return new PlayerData(playerMovement);
    }

    private void ApplySaveData(PlayerData data)
    {
        playerMovement.transform.position = data.position;
        playerMovement.transform.rotation = Quaternion.Euler(data.rotation);
        playerMovement.GasJumpKey = data.gasJumpKey;
        playerMovement.IceSlideKey = data.iceKey;
    }

}
