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
        To_TitleScene,
        To_UniqueScene,
        To_InGameScene,
    }

    [SerializeField]
    string _titleSceneName;
    [SerializeField]
    SceneTransitStatus _sceneStat;
    // 外部から覗かれるプロパティ
    public SceneTransitStatus SceneStatus { get { return _sceneStat; } set { _sceneStat = value; } }

    public string TitleSceneName { get { return _titleSceneName; } }

    protected override void ToDoAtAwakeSingleton()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Prolougue" || arg1.name == "Epilougue" || arg1.name == "StaffRoll")
            {
                _sceneStat = GameInfo.SceneTransitStatus.To_UniqueScene;
            }
            else if (arg1.name == "InGame" || arg1.name == "LastScene" || arg0.name == TitleSceneName)
            {
                _sceneStat = GameInfo.SceneTransitStatus.To_InGameScene;
            }
            else
            {
                _sceneStat = GameInfo.SceneTransitStatus.To_TitleScene;
            }
        }
    }
}
