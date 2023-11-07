using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateMachineUser : MonoBehaviour, IStateMachineUser
{
    StateMachine machine = new();
    A a = new A();
    B b = new B();
    C c = new C();
    // 遷移の条件判定部
    public void OnStateWasExitted(StateTransitionInfo info)
    {
        /* 呼び出しテストOK 11/07 */
        if(info._current == a &&  info._next == b)
        {
            Debug.Log("A => B");
            machine.GotoNextState();
        }
        if(info._current == b && info._next == c)
        {
            Debug.Log("B => C");
            machine.GotoNextState();
        }
        if(info._current == c && info._next == a)
        {
            Debug.Log("C => A");
            machine.GotoNextState();
        }
    }
    private void Awake()
    {
        machine.onStateExit += OnStateWasExitted;
        machine.AddTransition(a, b);
        machine.AddTransition(b, c);
        machine.AddTransition(c, a);
    }
    private void Update()
    {
        machine.Update();
    }
    private void OnDisable()
    {
        machine.onStateExit -= OnStateWasExitted;
        machine.ClearTransition();
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
public class C : IAIState
{
    public void In()
    {
        Debug.Log("C::Entry");
    }

    public void Out()
    {
        Debug.Log("C::Exit");
    }

    public void Tick()
    {
        Debug.Log("C::Tick");
    }
}