using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLib;
using UnityEngine.InputSystem;

public class InputBufferSample : MonoBehaviour
{
    PlayerInputBinder _inputBinder;

    Vector2 _mInput, _lInput;

    bool _fire;

    private void Awake()
    {
        _inputBinder = GetComponent<PlayerInputBinder>();

        _inputBinder.BindAxis("Player", "Move",
            (InputAction.CallbackContext context) =>
            {
                _mInput = context.ReadValue<Vector2>();
            }
            , ActionInvokeFaze.Performed);

        _inputBinder.BindAxis("Player", "Look",
            (InputAction.CallbackContext context) =>
            {
                _lInput = context.ReadValue<Vector2>();
            }, ActionInvokeFaze.Performed);

        _inputBinder.BindAction("Player", "Fire",
            (InputAction.CallbackContext context) =>
            {
                if (context.ReadValueAsButton())
                {
                    _fire = true;
                }
            },
            (InputAction.CallbackContext context) =>
            {
                if (!context.ReadValueAsButton())
                {
                    _fire = false;
                }
            },
            (InputAction.CallbackContext context) => { }
            );
    }

    private void Update()
    {
        //Debug.Log($"Move : {_mInput.ToString()}, Look : {_lInput.ToString()}, Fire : {_fire}");
    }
}
