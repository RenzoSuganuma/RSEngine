using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLib;
using UnityEngine.InputSystem;
public class InputBuffer : MonoBehaviour
{
    InputWindow _inputW;

    Vector2 _mInput, _lInput;

    bool _fire;

    private void Awake()
    {
        _inputW = GetComponent<InputWindow>();

        _inputW.BindAxis("Player", "Move",
            (InputAction.CallbackContext context) => 
            {
                _mInput = context.ReadValue<Vector2>();
            }, ActionInvokeFaze.Performed);
        
        _inputW.BindAxis("Player", "Look",
            (InputAction.CallbackContext context) => 
            {
                _lInput = context.ReadValue<Vector2>();
            }, ActionInvokeFaze.Performed);

        _inputW.BindAction("Player", "Fire",
            (InputAction.CallbackContext context) =>
            {
                _fire = context.ReadValueAsButton();
            });
    }

    private void Update()
    {
        Debug.Log($"Move : {_mInput.ToString()}, Look : {_lInput.ToString()}, Fire : {_fire}");
    }
}
