using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineUser : MonoBehaviour
{
    StateMachine machine = new();
    A a = new A();
    B b = new B();
    private void Awake()
    {
        machine.AddTransition(a, b);
        machine.AddTransition(b, a);
    }
    private void Update()
    {
        machine.Update();
    }
}
public class A : IAIState
{
    public void Entry()
    {
        Debug.Log("A::Entry");
    }

    public void Exit()
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
    public void Entry()
    {
        Debug.Log("B::Entry");
    }

    public void Exit()
    {
        Debug.Log("B::Exit");
    }

    public void Tick()
    {
        Debug.Log("B::Tick");
    }
}