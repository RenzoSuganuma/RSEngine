using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLib;
using UnityEngine.InputSystem;
using System.Linq;

public enum DeviceInputType
{
    Move,
    Look,
    Fire,
    Toggle,
}

public struct DeviceInputData
{
    InputAction.CallbackContext _context;
    public InputAction.CallbackContext Context { get { return _context; } }
    DeviceInputType _deviceInputType;
    public DeviceInputType DeviceInputType { get { return _deviceInputType; } }

    public DeviceInputData(InputAction.CallbackContext context, DeviceInputType type)
    {
        _context = context;
        _deviceInputType = type;
    }
}

public class InputBufferSample : MonoBehaviour
{
    PlayerInputBinder _inputBinder;

    Vector2 _mInput, _lInput;

    bool _fire;

    List<DeviceInputData> _inputHistory = new List<DeviceInputData>();

    private void Awake()
    {
        _inputBinder = GetComponent<PlayerInputBinder>();

        _inputBinder.BindAxis("Player", "Move",
            (InputAction.CallbackContext context) =>
            {
                //_mInput = context.ReadValue<Vector2>();

                DeviceInputData deviceInput = new DeviceInputData(context, DeviceInputType.Move);

                _inputHistory.Add(deviceInput);
            }
            , ActionInvokeFaze.Performed);

        _inputBinder.BindAxis("Player", "Look",
            (InputAction.CallbackContext context) =>
            {
                //_lInput = context.ReadValue<Vector2>();

                DeviceInputData deviceInput = new DeviceInputData(context, DeviceInputType.Look);

                _inputHistory.Add(deviceInput);
            }, ActionInvokeFaze.Performed);

        _inputBinder.BindAction("Player", "Fire",
            (InputAction.CallbackContext context) =>
            {
                if (context.ReadValueAsButton())
                {
                    //_fire = true;
                    DeviceInputData deviceInput = new DeviceInputData(context, DeviceInputType.Fire);

                    _inputHistory.Add(deviceInput);
                }
            },
            (InputAction.CallbackContext context) =>
            {
                if (!context.ReadValueAsButton())
                {
                    //_fire = false;
                    DeviceInputData deviceInput = new DeviceInputData(context, DeviceInputType.Fire);

                    _inputHistory.Add(deviceInput);
                }
            },
            (InputAction.CallbackContext context) => { }
            );
    }

    private void Update()
    {

        if (_inputHistory.Count > 0)
        {
            DeviceInputData InputData = _inputHistory[0];
            _inputHistory.RemoveAt(0);

            InputAction.CallbackContext context = InputData.Context;

            Vector2 move = Vector2.zero;
            Vector2 look = Vector2.zero;
            bool fire = false;

            switch (InputData.DeviceInputType)
            {
                case DeviceInputType.Move:
                    move = context.ReadValue<Vector2>();
                    break;
                case DeviceInputType.Look:
                    look = context.ReadValue<Vector2>();
                    break;
                case DeviceInputType.Fire:
                    fire = context.ReadValueAsButton();
                    break;
                case DeviceInputType.Toggle:
                    break;
                default: break;
            }

            Debug.Log($"Move Input[{move.ToString()}] , Look Input[{look.ToString()}], Fire Input [{fire.ToString()}]");
        }

    }
}
