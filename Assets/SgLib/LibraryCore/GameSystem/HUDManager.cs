using SgLib.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// auth suganuma
namespace SgLib
{
    namespace Systems
    {
        /// <summary> list ìoò^Ç≥ÇÍÇΩHUDÇä«óùÇ∑ÇÈ </summary>
        public class HUDManager : SingletonBaseClass<HUDManager>     // list ç≈å„îˆÇ™àÍî‘å„ÇÎ
        {
            [SerializeField, Header("The Object All HUD's Parent")]
            GameObject allHUDParent;
            [SerializeField, Header("Each HUD DO NOT RE ORDER")]
            List<GameObject> hud;

            public List<GameObject> HUDs => hud;

            SceneLoader _sLoader;

            public void ToFront(int index)
            {
                var huds = allHUDParent.GetChildObjects();
                for (int i = 0; i < allHUDParent.transform.childCount; i++)
                {
                    huds[i].gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
                }
                hud[index].transform.SetAsLastSibling();
                hud[index].gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            }

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
