using SLib.Devicies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ì¬ ›À
namespace SLib
{
    namespace Devicies
    {
        [CreateAssetMenu(fileName = "PadRamblingPreset", menuName = "GamepadRamblingPreset", order = 1)]
        public class GamepadRamblePreset : ScriptableObject
        {
            /// <summary> RumbleInfo ¶‚©‚ç ¶‚ÌU“® ‰E‚ÌU“® U“®‚ÌŠÔ </summary>
            [SerializeField] List<RumbleInfo<float, float, float>> _rumbleTable;
            public List<RumbleInfo<float, float, float>> Rumbles { get { return _rumbleTable; } }
        }
    }
}