using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
namespace SLib
{
    namespace Device
    {
        public class GamepadRumbler : MonoBehaviour
        {
            Gamepad _gamepad;

            [SerializeField] float lSpd;
            [SerializeField] float rSpd;

            //public GamepadRumbler(Gamepad gamepad)
            //{
            //    _gamepad = gamepad;
            //}

            private void Start()
            {
                _gamepad = Gamepad.current;
            }

            public void Rumble(float leftSpeed, float rightSpeed)
            {
                _gamepad.SetMotorSpeeds(leftSpeed, rightSpeed);
            }
        }
    }
}