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
    public delegate void OnStateExit(StateTransitionInfo info);
    public event OnStateExit onStateExit;
    int _currentTransitionIndex = 0; // 現在の遷移のリストからのインデックス
    public void Update()
    {
        // 現在エントリーしているステートの実行
        _transition[_currentTransitionIndex]._current.In();
        _transition[_currentTransitionIndex]._current.Tick();
        _transition[_currentTransitionIndex]._current.Out();
        //
        _transition[_currentTransitionIndex]._next.In();
        _transition[_currentTransitionIndex]._next.Tick();
        _transition[_currentTransitionIndex]._next.Out();
        // 
        var trans = _transition[_currentTransitionIndex];
        onStateExit(new StateTransitionInfo(trans._current, trans._next));
        // もし条件を満たしたのなら次の遷移先のステートへとぶ
        var work = FindNextEntryPoint(_transition[_currentTransitionIndex]._next);
        _currentTransitionIndex = (work != -1) ? work : _currentTransitionIndex; // もし、どこにもいかないのならそのステートにとどまる
    }
    public void AddTransition(IAIState current, IAIState next)
    {
        _transition.Add(new StateTransition<IAIState, IAIState>(current, next));
    }
    public void GotoNextState()
    {

    }
    // 渡された StateTransition.next のステートがStateTransition.current として登録されている遷移リストのインデックスを返す
    int FindNextEntryPoint(IAIState next)
    {
        int index = -1;
        for (int i = 0; i < _transition.Count; i++)
        {
            if (_transition[i]._current == next)
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
public struct StateTransitionInfo
{
    public IAIState _current;
    public IAIState _next;
    public StateTransitionInfo(IAIState current, IAIState next)
    {
        _current = current;
        _next = next;
    }
}
public interface IAIState
{
    public void In();
    public void Tick();
    public void Out();
}
public interface IStateMachineUser
{
    public void OnStateWasExitted(StateTransitionInfo info);
}