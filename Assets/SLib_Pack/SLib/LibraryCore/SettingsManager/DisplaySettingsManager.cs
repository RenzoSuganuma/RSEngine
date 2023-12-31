using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 担当 菅沼
// ディスプレイのデバイス名とリフレッシュレートの変更はできたっぽい ← ここまでは動作確認できている
namespace SLib
{
    public class DisplaySettingsManager : MonoBehaviour
    {
        [SerializeField]
            Text _text;

        public string GetResolution()
        {
            return $"{Screen.currentResolution.ToString()}";
        }

        public string GetDisplayName(int index)
        {
            List<DisplayInfo> list = new List<DisplayInfo>();
            Screen.GetDisplayLayout(list);
            return list[index].name;
        }

        public void SetDisplayResolutions(string resolution)
        {
            int width;
            int height;
            width = int.Parse(resolution.Split()[0]);
            height = int.Parse(resolution.Split()[1]);

            Screen.SetResolution(width, height, true, 60);
        }

        public int GetRefreshRate()
        {
            return Application.targetFrameRate;
        }

        public void SetRefreshRate(int rate)
        {
            Application.targetFrameRate = rate;
        }
    }
}
