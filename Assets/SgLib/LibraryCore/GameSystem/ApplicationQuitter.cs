using SgLib.Singleton;
using System;
using UnityEditor;
using UnityEngine;
// �쐬 �����ʂ�
namespace SgLib
{
    namespace Systems
    {
        public class ApplicationQuitter : SingletonBaseClass<ApplicationQuitter>
        {
            [SerializeField, Header("Player Tag")]
            string PlayerTag;

            event Action<GameInfo.SceneTransitStatus> eventOnTransit;

            public event Action<GameInfo.SceneTransitStatus>
                EventOnTransit
            {
                add { eventOnTransit += value; }
                remove { eventOnTransit -= value; }
            }

            Transform _pTrans;
            GameInfo _gInfo;

            protected override void ToDoAtAwakeSingleton()
            {
                _gInfo = GameObject.FindFirstObjectByType<GameInfo>();
            }
            public void QuitApplication()
            {
                #region PreProcess
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
                #endregion

                eventOnTransit(_gInfo.GetSceneStatus);

                Application.Quit();
            }
        }

    }
}
