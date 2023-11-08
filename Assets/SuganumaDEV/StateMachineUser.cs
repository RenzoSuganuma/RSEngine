using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSEngine.AI;
public class StateMachineUser : MonoBehaviour, IStateMachineUser
{
    StateMachine machine = new();// Machine 
    // Users 
    A a = new A();
    B b = new B();
    C c = new C();
    // Cashsed Bool
    bool b1 = false;
    bool b2 = false;
    // 遷移の条件判定部
    public void OnStateWasExitted(StateTransitionInfo info)
    {
        /* 呼び出しテストOK 11/07 */

        if(info._current == a &&  info._next == b)
        {
            Debug.Log("A => B");
        }if(info._current == b &&  info._next == c)
        {
            Debug.Log("B => C");
        }
    }
    private void Awake()
    {
        machine.onStateExit += OnStateWasExitted;
        machine.AddTransition(a, b); // a => b
        machine.AddTransition(b, c); // b => c
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

    private void OnGUI()
    {
        b1 = GUI.Toggle(new Rect(0, 0, 100, 100), b1, "State A Ready");
        if(b1)
        {
            Debug.Log("STate A Im Goto Next");
            a.SendMessageGotoNext();
        }
        b2 = GUI.Toggle(new Rect(0, 100, 100, 100), b2, "State B Ready");
        if (b2)
        {
            Debug.Log("State B Im Goto Next");
            b.SendMessageGotoNext();
        }
    }
}
public class A : IAIState
{
    bool ready = false;
    public void SendMessageGotoNext()
    {
        ready = true;
    }

    public void In()
    {
        Debug.Log("A::Entry");
    }

    public void Out()
    {
        Debug.Log("A::Exit");
    }

    public bool ReadyToGoNext()
    {
        return ready;
    }

    public void Tick()
    {
        Debug.Log("A::Tick");
    }
}
public class B : IAIState
{
    bool ready = false;
    public void SendMessageGotoNext()
    {
        ready = true;
    }

    public void In()
    {
        Debug.Log("B::Entry");
    }

    public void Out()
    {
        Debug.Log("B::Exit");
    }

    public bool ReadyToGoNext()
    {
        return ready;
    }

    public void Tick()
    {
        Debug.Log("B::Tick");
    }
}
public class C : IAIState
{
    bool ready;
    public void SendMessageGotoNext()
    {
        ready = true;
    }

    public void In()
    {
        Debug.Log("C::Entry");
    }

    public void Out()
    {
        Debug.Log("C::Exit");
    }

    public bool ReadyToGoNext()
    {
        return ready;
    }

    public void Tick()
    {
        Debug.Log("C::Tick");
    }
}