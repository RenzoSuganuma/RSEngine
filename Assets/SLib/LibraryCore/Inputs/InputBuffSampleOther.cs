using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SgLib.Inputs;
public class InputBuffSampleOther : MonoBehaviour
{
    InputBuffer _buffer;

    private void Start()
    {
        _buffer = GetComponent<InputBuffer>();
    }

    private void FixedUpdate()
    {
        var context = _buffer.EnQueueInputContext();

        var move = context.move;
        var look = context.look;
        var fire = context.fire;
        var toggle = context.toggle;

        Debug.Log($"move {move.ToString()}");
        Debug.Log($"look {look.ToString()}");
        Debug.Log($"fire {fire.ToString()}");
    }
}
