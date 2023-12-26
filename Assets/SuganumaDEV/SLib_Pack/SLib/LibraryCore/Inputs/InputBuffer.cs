using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLib;
using UnityEngine.InputSystem;
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

public class InputBuffer : MonoBehaviour
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
                QueueInput(context, DeviceInputType.Move);
            }
            , ActionInvokeFaze.Performed);

        _inputBinder.BindAxis("Player", "Look",
            (InputAction.CallbackContext context) =>
            {
                QueueInput(context, DeviceInputType.Look);
            }, ActionInvokeFaze.Performed);

        _inputBinder.BindAction("Player", "Fire",
            (InputAction.CallbackContext context) =>
            {
                if (context.ReadValueAsButton())
                {
                    QueueInput(context, DeviceInputType.Fire);
                }
            },
            (InputAction.CallbackContext context) =>
            {
                if (!context.ReadValueAsButton())
                {
                    QueueInput(context, DeviceInputType.Fire);
                }
            },
            (InputAction.CallbackContext context) => { }
            );
    }

    private void Update()
    {
        Vector2 move = Vector2.zero;
        Vector2 look = Vector2.zero;
        bool fire = false;

        var temp = EnQueueInput();

        move = temp.move; 
        look = temp.look;
        fire = temp.fire;

        Debug.Log($"Move Input[{move.ToString()}] , Look Input[{look.ToString()}], Fire Input [{fire.ToString()}]");
    }

    public void QueueInput(InputAction.CallbackContext context, DeviceInputType inputType)
    {
        DeviceInputData deviceInput;
        switch (inputType)
        {
            case DeviceInputType.Move:
                deviceInput = new DeviceInputData(context, DeviceInputType.Move);
                _inputHistory.Add(deviceInput);
                break;
            case DeviceInputType.Look:
                deviceInput = new DeviceInputData(context, DeviceInputType.Look);
                _inputHistory.Add(deviceInput);
                break;
            case DeviceInputType.Fire:
                deviceInput = new DeviceInputData(context, DeviceInputType.Fire);
                _inputHistory.Add(deviceInput);
                break;
        }
    }

    public (Vector2 move, Vector2 look, bool fire) EnQueueInput()
    {
        Vector2 move = Vector2.zero;
        Vector2 look = Vector2.zero;
        bool fire = false;

        if (_inputHistory.Count > 0)
        {
            DeviceInputData InputData = _inputHistory[0];
            _inputHistory.RemoveAt(0);

            InputAction.CallbackContext context = InputData.Context;

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
        }
        return (move, look, fire);
    }
}
