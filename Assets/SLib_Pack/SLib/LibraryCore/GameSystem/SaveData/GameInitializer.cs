using SLib.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLib.Systems;
// ì¬ ›À
public class GameInitializer : SingletonBaseClass<GameInitializer>
{
    [SerializeField]
    Transform _playerTransform;

    PlayerSaveDataSerializer _dataSerializer;

    protected override void ToDoAtAwakeSingleton()
    {
       _dataSerializer = GameObject.FindObjectOfType<PlayerSaveDataSerializer>();
    }

    public void InitializePlayer()      // ƒQ[ƒ€ƒV[ƒ““Ç‚İ‚İŒã‚É‚±‚ê‚ğ“Ç‚İ‚Ş
    {
        SaveDataTemplate saveDataTemplate = _dataSerializer.ReadSaveData();
        _playerTransform.position = saveDataTemplate._lastStandingPosition;
        _playerTransform.rotation = saveDataTemplate._lastStandingRotation;
        print($"{saveDataTemplate._lastStandingPosition.ToString()} : {saveDataTemplate._lastStandingRotation.ToString()}");
    }
}
