using SLib.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLib.Systems;
// 作成 菅沼
public class GameInitializer : SingletonBaseClass<GameInitializer>
{
    [SerializeField]
    Transform _playerTransform;

    PlayerSaveDataSerializer _dataSerializer;

    protected override void ToDoAtAwakeSingleton()
    {
       _dataSerializer = GameObject.FindObjectOfType<PlayerSaveDataSerializer>();
    }

    public void InitializePlayer()      // ゲームシーン読み込み後にこれを読み込む
    {
        SaveDataTemplate saveDataTemplate = _dataSerializer.ReadSaveData();
        _playerTransform.position = saveDataTemplate._lastStandingPosition;
        _playerTransform.rotation = saveDataTemplate._lastStandingRotation;
        print($"{saveDataTemplate._lastStandingPosition.ToString()} : {saveDataTemplate._lastStandingRotation.ToString()}");
    }
}
