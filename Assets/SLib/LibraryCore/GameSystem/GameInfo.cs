using SgLib.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// auth ����
public class GameInfo : SingletonBaseClass<GameInfo>
{
    /// <summary> �J�ڐ�V�[�����ǂ̂悤�Ȃ��̂���I���A�ێ����� </summary>
    public enum SceneTransitStatus
    {
        ToTitle,
        ToUnique,
        ToInGame,
    }

    [SerializeField]
    SceneTransitStatus sceneStatus;
    [SerializeField]
    List<string> titleSceneNames;
    [SerializeField]
    List<string> inGameSceneNames;
    [SerializeField]
    List<string> uniqueSceneNames;

    // �O������`�����v���p�e�B
    public SceneTransitStatus GetSceneStatus { get { return sceneStatus; }}

    public List<string> SetTitleSceneName { set { this.titleSceneNames = value; } }
    public List<string> SetInGameSceneName { set { this.inGameSceneNames = value; } }
    public List<string> SetUniqueSceneName { set { this.uniqueSceneNames = value; } }

    protected override void ToDoAtAwakeSingleton()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            #region [For-Each Loop] 
            /// !!!-2/6 ����null�Q�ƂŃo�O����������
            foreach (var name in titleSceneNames)
            {
                if (arg1.name == name)
                {
                    sceneStatus = SceneTransitStatus.ToTitle;
                    break;
                }
            }

            foreach (var name in inGameSceneNames)
            {
                if (arg1.name == name)
                {
                    sceneStatus = SceneTransitStatus.ToInGame;
                    break;
                }
            }

            foreach (var name in uniqueSceneNames)
            {
                if (arg1.name == name)
                {
                    sceneStatus = SceneTransitStatus.ToUnique;
                    break;
                }
            }
            /// !!!-
            #endregion
        }
    }
}
