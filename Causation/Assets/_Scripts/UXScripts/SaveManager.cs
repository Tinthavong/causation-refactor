using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public SaveData gameData;

    public bool hasLoaded;

    public bool hasSaved;

    public void Awake()
    {
        instance = this;

        string savePath = Application.persistentDataPath;

        if (File.Exists(savePath + "/" + gameData.saveName + ".dat"))
        {
            Load();
        }
    }

    public void Save()
    {
        string savePath = Application.persistentDataPath;

        var serializer = new XmlSerializer(typeof(SaveData));

        var stream = new FileStream(savePath + "/" + gameData.saveName + ".dat", FileMode.Create);

        serializer.Serialize(stream, gameData);

        stream.Close();

        Debug.Log("Game Saved");
    }

    public void Load()
    {
        string savePath = Application.persistentDataPath;

        if(File.Exists(savePath + "/" + gameData.saveName + ".dat"))
        {
            var serializer = new XmlSerializer(typeof(SaveData));

            var stream = new FileStream(savePath + "/" + gameData.saveName + ".dat", FileMode.Open);

            gameData = serializer.Deserialize(stream) as SaveData;

            stream.Close();

            Debug.Log("Game Loaded");

            hasLoaded = true;
        }
    }

    public void DeleteSavedData()
    {
        string savePath = Application.persistentDataPath;

        if (File.Exists(savePath + "/" + gameData.saveName + ".dat"))
        {
            File.Delete(savePath + "/" + gameData.saveName + ".dat");

            instance.gameData.iteration = 0;
            instance.gameData.currency = 0;
            

            Debug.Log("Saved data has been cleared");
        }
    }
}

[System.Serializable]
public class SaveData
{
    public string saveName;
    public int currency;
    public int iteration;
}