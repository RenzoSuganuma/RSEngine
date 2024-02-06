using SgLib.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// auth 菅沼
public class GameInfo : SingletonBaseClass<GameInfo>
{
    /// <summary> 遷移先シーンがどのようなものかを選択、保持する </summary>
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

    // 外部から覗かれるプロパティ
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
            /// !!!-2/6 ここnull参照でバグ発生源かも
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
