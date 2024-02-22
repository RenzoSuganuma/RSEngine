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
        public class HUDManager : SingletonBaseClass<HUDManager> // list 最後尾が一番後ろ
        {
            [SerializeField, Header("The Object All HUD's Parent")]
            GameObject allHUDParent;

            [SerializeField, Header("The Preset Of HUD")]
            HUDPreset hud;

            SceneLoader _sLoader;

            /// <summary> 最前面へ移動 </summary>
            public void ToFront(int index)
            {
                var huds = allHUDParent.GetChildObjects();
                for (int i = 0; i < allHUDParent.transform.childCount; i++)
                {
                    huds[i].gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                }

                hud.HUDList[index].transform.SetAsLastSibling();
                hud.HUDList[index].gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            }

            /// <summary> 最前面へ移動 </summary>
            public void ToFront(string hudObjectName)
            {
                var huds = allHUDParent.GetChildObjects();
                for (int i = 0; i < allHUDParent.transform.childCount; i++)
                    huds[i].gameObject.GetComponent<CanvasGroup>().alpha =
                        (huds[i].name == hudObjectName) ? 1.0f : 0.0f;
            }

            /// <summary> すべて非表示にする </summary>
            public void HideAll()
            {
                var huds = allHUDParent.GetChildObjects();
                foreach (var hud in huds)
                {
                    hud.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                }
            }

            protected override void ToDoAtAwakeSingleton()
            {
                GameObject.DontDestroyOnLoad(allHUDParent);
                _sLoader = GameObject.FindFirstObjectByType<SceneLoader>();
            }
        }
    }
}