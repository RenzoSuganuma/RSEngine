using SgLib.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

//�쐬 ����

namespace SgLib
{
    namespace Systems
    {
        [Serializable]
        public class SaveDataTemplate
        {
            public Vector3 lastStandingPosition;           // Pos
            public Quaternion lastLookingRotation;       // Rot
            public string sceneNameLastStand;                      // Scene Name
        }

        /// <summary> �n���ꂽ�t�B�[���h�̒l�����Ƃ�ScriptableObject�𐶐��ADataPath�����֊i�[�B </summary>
        public class SaveDataCreator : SingletonBaseClass<SaveDataCreator>  // �Z�[�u�f�[�^�̕ۑ�
        {
            [SerializeField, Header("The Tag That Player Has")]
            string PlayerTag;

            string _playerDataPath;
            string _dataFileName;
            public string SetFileName { set { _dataFileName = value; } }
            public string GetFileName { get { return _dataFileName; } }

            Transform _pTrans;

            GameInfo _gInfo;
            protected override void ToDoAtAwakeSingleton()
            {
                _gInfo = GameObject.FindFirstObjectByType<GameInfo>();
                _playerDataPath = Application.dataPath + "/" + _dataFileName + ".json";
            }


            public void SavePlayerDataAutomatically()
            {
                switch (_gInfo.GetSceneStatus)
                {
                    case GameInfo.SceneTransitStatus.ToTitle:
                        break;
                    case GameInfo.SceneTransitStatus.ToInGame:
                        if (_pTrans == null)
                        {
                            _pTrans = GameObject.FindGameObjectWithTag(PlayerTag).transform;
                        }
                        RunSaveData(_pTrans, SceneManager.GetActiveScene().name);
                        break;
                }
            }

            public void RunSaveData(Transform playerStandingTransform, string sceneName)
            {
                SaveDataTemplate template = new SaveDataTemplate();
                template.lastStandingPosition = playerStandingTransform.position;
                template.lastLookingRotation = playerStandingTransform.rotation;
                template.sceneNameLastStand = sceneName;

                string jsonStr = JsonUtility.ToJson(template);

                StreamWriter sw = new StreamWriter(_playerDataPath, false);
                sw.WriteLine(jsonStr);
                sw.Flush();
                sw.Close();
            }
        }
    }
}
