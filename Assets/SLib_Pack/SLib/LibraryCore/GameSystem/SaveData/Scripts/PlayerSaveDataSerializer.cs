using SLib.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerSaveDataSerializer : SingletonBaseClass<PlayerSaveDataSerializer> // セーブデータの展開
{
    protected override void ToDoAtAwakeSingleton() { }

    public void ReadFuckingData()
    {
        var data = ReadSaveData();
        Debug.Log($"{data._lastStandingTransform.position.ToString()}," +
            $"{data._lastStandingTransform.rotation.ToString()} = {data._sceneName}");
    }

    public SaveDataTemplate ReadSaveData()
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/PlayerSavedData.json");
        string dataStr = sr.ReadToEnd();
        sr.Close();
        return JsonUtility.FromJson<SaveDataTemplate>(dataStr);
    }
}
