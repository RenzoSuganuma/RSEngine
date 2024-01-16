using SLib.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveDataTemplate
{
    public Transform _lastStandingTransform;       // Transform
    public string _sceneName;                      // Scene Name
}

/// <summary> 渡されたフィールドの値をもとにScriptableObjectを生成、DataPath直下へ格納。 </summary>
public class PlayerSaveDataCreator : SingletonBaseClass<PlayerSaveDataCreator>  // セーブデータの保存
{
    protected override void ToDoAtAwakeSingleton() { }

    [SerializeField] Transform _tr;
    [SerializeField] string _sceneName;

    string _playerDataPath = Application.dataPath + "/PlayerSavedData.json";

    public void SaveDatas()
    {
        SavePlayerData(_tr, _sceneName);
    }

    public void SavePlayerData(Transform playerStandingTransform, string sceneName)
    {
        SaveDataTemplate template = new SaveDataTemplate();
        template._lastStandingTransform = playerStandingTransform;
        template._sceneName = sceneName;

        string jsonStr = JsonUtility.ToJson(template);

        StreamWriter sw = new StreamWriter(_playerDataPath, false);
        sw.WriteLine(jsonStr);
        sw.Flush();
        sw.Close();
    }
}
