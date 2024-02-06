using SgLib.Devicies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �쐬 ����
namespace SgLib
{
    namespace Devicies
    {
        [CreateAssetMenu(fileName = "PadRamblingPreset", menuName = "GamepadRamblingPreset", order = 1)]
        public class GamepadRamblePreset : ScriptableObject
        {
            /// <summary> RumbleInfo ������ ���̐U�� �E�̐U�� �U���̎��� </summary>
            [SerializeField] List<RumbleInfo<float, float, float>> rumbleTable;
            public List<RumbleInfo<float, float, float>> Rumbles { get { return rumbleTable; } }
        }
    }
}