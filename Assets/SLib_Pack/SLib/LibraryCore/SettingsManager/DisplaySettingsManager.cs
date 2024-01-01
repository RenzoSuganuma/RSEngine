using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 担当 菅沼
// ディスプレイのデバイス名とリフレッシュレートの変更はできたっぽい ← ここまでは動作確認できている
// Player設定 ＞ FullScreen モード 、 Default Is Native Resolution = false この設定は必ずすること
namespace SLib
{
    public class DisplaySettingsManager : MonoBehaviour
    {
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

        /// <summary> 空白区切りでピクセル数の指定をする </summary>
        /// <param name="resolution"></param>
        public void SetDisplayResolutions(string resolution)
        {
            int width;
            int height;
            width = int.Parse(resolution.Split()[0]);
            height = int.Parse(resolution.Split()[1]);

            Screen.SetResolution(width, height, true);
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
