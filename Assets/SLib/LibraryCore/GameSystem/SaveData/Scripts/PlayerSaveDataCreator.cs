using SgLib.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//作成 菅沼

namespace SgLib
{
    namespace Systems
    {
        [Serializable]
        public class SaveDataTemplate
        {
            public Vector3 _lastStandingPosition;           // Pos
            public Quaternion _lastStandingRotation;       // Rot
            public string _sceneName;                      // Scene Name
        }

        /// <summary> 渡されたフィールドの値をもとにScriptableObjectを生成、DataPath直下へ格納。 </summary>
        public class PlayerSaveDataCreator : SingletonBaseClass<PlayerSaveDataCreator>  // セーブデータの保存
        {
            Transform _playerTransform;

            GameInfo _gameInfo;
            protected override void ToDoAtAwakeSingleton()
            {
                _gameInfo = GameObject.FindFirstObjectByType<GameInfo>();
            }

            string _playerDataPath = Application.dataPath + "/PlayerSavedData.json";

            public void SavePlayerDataAutomatically()
            {
                switch (_gameInfo.SceneStatus)
                {
                    case GameInfo.SceneTransitStatus.To_TitleScene:
                        break;
                    case GameInfo.SceneTransitStatus.To_InGameScene:
                        if (_playerTransform == null)
                        {
                            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                        }
                        SavePlayerData(_playerTransform, SceneManager.GetActiveScene().name);
                        break;
                }
            }

            public void SavePlayerData(Transform playerStandingTransform, string sceneName)
            {
                SaveDataTemplate template = new SaveDataTemplate();
                template._lastStandingPosition = playerStandingTransform.position;
                template._lastStandingRotation = playerStandingTransform.rotation;
                template._sceneName = sceneName;

                string jsonStr = JsonUtility.ToJson(template);

                StreamWriter sw = new StreamWriter(_playerDataPath, false);
                sw.WriteLine(jsonStr);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
