using SLib.Singleton;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
// çÏê¨ Ç∑Ç™Ç Ç‹
namespace SLib
{
    namespace Systems
    {
        public class ApplicationQuitter : SingletonBaseClass<ApplicationQuitter>
        {
            [SerializeField]
            Transform _playerTransform;

            PlayerSaveDataCreator _playerSaveDataCreator;

            protected override void ToDoAtAwakeSingleton()
            {
                _playerSaveDataCreator = GameObject.FindFirstObjectByType<PlayerSaveDataCreator>();
            }

            public void QuitApplication()
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                _playerSaveDataCreator.SavePlayerData(_playerTransform, SceneManager.GetActiveScene().name);
                Application.Quit();
            }
        }

    }
}
