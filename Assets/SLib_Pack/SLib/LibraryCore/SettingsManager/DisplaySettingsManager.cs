using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
