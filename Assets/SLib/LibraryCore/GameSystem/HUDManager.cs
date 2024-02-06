using SgLib.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// auth suganuma
namespace SgLib
{
    namespace Systems
    {
        /// <summary> list 登録されたHUDを管理する </summary>
        public class HUDManager : SingletonBaseClass<HUDManager>     // list 最後尾が一番後ろ
        {
            [SerializeField, Header("各HUDの親オブジェクト")]
            GameObject _allHUDParent;
            [SerializeField, Header("各HUDモジュール \n＃モジュールを＋で追加すること。絶対に並び変えない＃")]
            List<GameObject> _huds;

            public List<GameObject> HUDs => _huds;

            SceneLoader _sceneLoader;

            public void ToFront(int index)
            {
                var huds = _allHUDParent.GetChildObjects();
                for (int i = 0; i < _allHUDParent.transform.childCount; i++)
                {
                    huds[i].gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                }
                _huds[index].transform.SetAsLastSibling();
                _huds[index].gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            }

            public void KillAll()
            {
                var huds = _allHUDParent.GetChildObjects();
                foreach (var hud in huds)
                {
                    hud.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                }
            }

            protected override void ToDoAtAwakeSingleton()
            {
                GameObject.DontDestroyOnLoad(_allHUDParent);
                _sceneLoader = GameObject.FindFirstObjectByType<SceneLoader>();
            }
        }
    }
}
