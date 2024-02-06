using SgLib.Singleton;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
#if false
using DG.Tweening;
#endif
// Auth : Suganuma
namespace SgLib
{
    namespace Systems
    {
        public class SceneLoader : SingletonBaseClass<SceneLoader>
        {
            [SerializeField, Header("Now Loading 表示のパネル")]
            GameObject _nowLoadingPanel;
            [SerializeField, Header("ローディングのテキスト")]
            Text _loadingText;
            [SerializeField, Header("シーン遷移時に必ず発火されるイベント")]
            public UnityEvent<Scene> _eventOnSceneLoaded;

            public void LoadSceneByName(string sceneName)
            {
                StartCoroutine(LoadSceneAcyncByName(sceneName));
            }

            protected override void ToDoAtAwakeSingleton()
            {
                _nowLoadingPanel.SetActive(false);
                _nowLoadingPanel.transform.SetAsFirstSibling();
                SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            }

            void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
            {
                _eventOnSceneLoaded.Invoke(arg1);   // 他クラスから

                _nowLoadingPanel.transform.SetAsFirstSibling();
                _nowLoadingPanel.SetActive(false);
            }

            IEnumerator LoadSceneAcyncByName(string sceneName)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
                while (!asyncLoad.isDone)
                {
                    _nowLoadingPanel.transform.SetAsLastSibling();
                    _nowLoadingPanel.SetActive(!false);
#if false
                    _loadingText.DOText("Loading...", 1);
#endif
                    yield return null;
                }
            }
        }

        public interface IOnSceneTransit
        {
            public void OnSceneTransitComplete(Scene scene);
        }
    }
}
