using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineUser : MonoBehaviour, IStateMachineUser
{
    StateMachine machine = new();
    A a = new A();
    B b = new B();
    public void OnStateWasExitted(StateTransitionInfo info)
    {
        /* 呼び出しテストOK 11/07 */
        if(info._current == a &&  info._next == b && info._isCurrentStateNow)
        {
            machine.GotoNextState();
        }
    }
    private void Awake()
    {
        machine.onStateExit += OnStateWasExitted;
        machine.AddTransition(a, b);
        //machine.AddTransition(b, a);
    }
    private void Update()
    {
        machine.Update();
    }
    private void OnDisable()
    {
        machine.onStateExit -= OnStateWasExitted;
    }
}
public class A : IAIState
{
    public void In()
    {
        Debug.Log("A::Entry");
    }

    public void Out()
    {
        Debug.Log("A::Exit");
    }

    public void Tick()
    {
        Debug.Log("A::Tick");
    }
}
public class B : IAIState
{
    public void In()
    {
        Debug.Log("B::Entry");
    }

    public void Out()
    {
        Debug.Log("B::Exit");
    }

    public void Tick()
    {
        Debug.Log("B::Tick");
    }
}