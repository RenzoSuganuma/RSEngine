using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ステートマシン 
// もし登録した遷移を満たす条件が合えば遷移する
// 
public class StateMachine
{
    List<StateTransition<IAIState, IAIState>> _transition = new();
    int _current = 0;
    public void Update()
    {
        _transition[_current]._current.Entry();
        _transition[_current]._current.Tick();
        _transition[_current]._current.Exit();
        _current = FindNextEntryPoint(_transition[_current]._next);
    }
    public void AddTransition(IAIState current, IAIState next)
    {
        _transition.Add(new StateTransition<IAIState, IAIState>(current, next));
    }
    // 渡された StateTransition.next のステートがStateTransition.current として登録されている遷移リストのインデックスを返す
    int FindNextEntryPoint(IAIState next)
    {
        int index = -1;
        for (int i = 0; i < _transition.Count; i++)
        {
            if(_transition[i]._current == next) 
            {
                index = i; 
                break;
            }
        }
        return index;
    }
}
// 遷移元と遷移先
public class StateTransition<Tcurrent, Tnext>
{
    public StateTransition(Tcurrent current, Tnext next)
    {
        _current = current;
        _next = next;
    }
    public Tcurrent _current;
    public Tnext _next;
}
public interface IAIState
{
    public void Entry();
    public void Tick();
    public void Exit();
}