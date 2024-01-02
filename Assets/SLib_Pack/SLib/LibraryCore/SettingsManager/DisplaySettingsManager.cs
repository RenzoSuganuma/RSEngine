using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
// 担当 菅沼
// ディスプレイのデバイス名とリフレッシュレートの変更はできたっぽい ← ここまでは動作確認できている
// アクティブなディスプレイ切り替え機能 実装 OK ← 動作確認 OK
// Player設定 ＞ FullScreen モード 、 Default Is Native Resolution = false この設定は必ずすること
namespace SLib
{
    public class DisplaySettingsManager : MonoBehaviour
    {
        [SerializeField]
        Dropdown _displaysDD;
        [SerializeField]
        Dropdown _resolutionsDD;
        [SerializeField]
        Dropdown _refreshRateDD;

        /// <summary> 解像度の一覧。Key -> 表示するのに取得、Value -> 関数に渡す </summary>
        Dictionary<string, string> ResolutionsList = new()
        {
            {"FHD [1920×1080 16:9]" , "1920 1080"},
            {"WSXGA [1680×1050 16:10]" , "1680 1050"},
            {"WQHD [2560×1440 16:9]" , "2560 1440"},
            {"WQXGA [2560×1600 16:10]" , "2560 1600"},
        };

        /// <summary> リフレッシュレートの一覧。Key -> 表示するのに取得、Value -> 関数に渡す </summary>
        Dictionary<string, int> RefreshRateList = new()
        {
            {"60Hz" , 60},
            {"75Hz", 75 },
            {"120Hz", 120 },
            {"144Hz" , 144},
            {"165Hz", 165 }
        };

        #region ScriptFunctions

        /// <summary> DropDownOnValueChanged </summary>
        /// <param name="dropdown"></param>
        /// <returns></returns>
        int DDOnValueChanged(Dropdown dropdown)
        {
            return dropdown.value;
        }

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

        public List<DisplayInfo> GetDisplays()
        {
            List<DisplayInfo> list = new List<DisplayInfo>();
            Screen.GetDisplayLayout(list);
            return list;
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

        #endregion

        public void ChangeGameDisplay(Dropdown dropdown)
        {
            int displayIndex = DDOnValueChanged(dropdown);
            var displays = GetDisplays();
            var display = displays[displayIndex];
            Screen.MoveMainWindowTo(display, display.workArea.position);
        }

        public void ChangeGameResolutions(Dropdown dropdown)
        {
            int resolutionIndex = DDOnValueChanged(dropdown);
            var resolutionRaw = ResolutionsList.Values.ToList();
            var resolution = resolutionRaw[resolutionIndex];
            SetDisplayResolutions(resolution);
        }
        
        public void ChangeGameRefreshRate(Dropdown dropdown)
        {
            int rateIndex = DDOnValueChanged(dropdown);
            var rateRaw = RefreshRateList.Values.ToList();
            var rate = rateRaw[rateIndex];
            SetRefreshRate(rate);
        }

        void SetupActiveDisplaysDropdown()  // アクティブなディスプレイをドロップダウンへ名前のみ渡す
        {
            _displaysDD.options.Clear();
            var displays = GetDisplays();
            List<Dropdown.OptionData> optionData = new();
            foreach (var display in displays)
            {
                var data = new Dropdown.OptionData();
                data.text = display.name;
                optionData.Add(data);
            }
            _displaysDD.options = optionData;
        }

        void SetupResolutionsDropDown()
        {
            _resolutionsDD.options.Clear();
            List<Dropdown.OptionData> optionData = new();
            var resolutions = ResolutionsList.Keys.ToList();
            foreach (var resolution in resolutions)
            {
                Dropdown.OptionData data = new Dropdown.OptionData();
                data.text = resolution;
                optionData.Add(data);
            }
            _resolutionsDD.options = optionData;
        }
        
        void SetupRefreshRateDropDown()
        {
            _refreshRateDD.options.Clear();
            List<Dropdown.OptionData> optionData = new();
            var rates = RefreshRateList.Keys.ToList();
            foreach (var rate in rates)
            {
                Dropdown.OptionData data = new Dropdown.OptionData();
                data.text = rate;
                optionData.Add(data);
            }
            _refreshRateDD.options = optionData;
        }

        private void Start()
        {
            SetupActiveDisplaysDropdown();
            SetupResolutionsDropDown();
            SetupRefreshRateDropDown();
        }
    }
}
