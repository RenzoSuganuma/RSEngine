using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// ì¬ F ›À
namespace SLib
{
    namespace Device
    {
        public class GamepadRumbler : MonoBehaviour
        {
            Gamepad _gamepad;
            /// <summary> RumbleInfo ¶‚©‚ç ¶‚ÌU“® ‰E‚ÌU“® U“®‚ÌŠÔ </summary>
            [SerializeField] List<RumbleInfo<float, float, float>> _rumbleTable;

            private void Start()
            {
                _gamepad = Gamepad.current;
            }

            public void Rumble(float leftSpeed, float rightSpeed)
            {
                _gamepad.SetMotorSpeeds(leftSpeed, rightSpeed);
            }

            public IEnumerator RumbleRoutine(float leftSpeed, float rightSpeed, float rumbleTime)
            {
                _gamepad.SetMotorSpeeds(leftSpeed, rightSpeed);

                yield return new WaitForSeconds(rumbleTime);

                _gamepad.SetMotorSpeeds(0f, 0f);
            }

            public IEnumerator RumbleRoutine(float leftSpeed, float rightSpeed, float rumbleTime, int repeatTimes)
            {
                for (int i = 0; i < repeatTimes; i++)
                {
                    _gamepad.SetMotorSpeeds(leftSpeed, rightSpeed);

                    yield return new WaitForSeconds(rumbleTime);

                    _gamepad.SetMotorSpeeds(0f, 0f);
                }
            }

            public IEnumerator RumbleByTable()
            {
                foreach (var table in _rumbleTable)
                {
                    _gamepad.SetMotorSpeeds(table.LeftStrength, table.RightStrength);

                    yield return new WaitForSeconds(table.Time);

                    _gamepad.SetMotorSpeeds(0f, 0f);
                }
            }
        }

        [Serializable]
        public class RumbleInfo<T, T1, T2>
        {
            [SerializeField] T _strengthL;
            public T LeftStrength { get { return _strengthL; } }
            [SerializeField] T1 _strengthR;
            public T1 RightStrength { get { return _strengthR; } }
            [SerializeField] T2 _rumblingTime;
            public T2 Time { get { return _rumblingTime; } }

            public RumbleInfo(T leftStrength, T1 rightStrength, T2 rumbleTime)
            {
                _strengthL = leftStrength;
                _strengthR = rightStrength;
                _rumblingTime = rumbleTime;
            }
        }
    }
}