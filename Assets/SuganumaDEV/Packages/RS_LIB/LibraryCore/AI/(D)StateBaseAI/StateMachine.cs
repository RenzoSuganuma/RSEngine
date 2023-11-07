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
    delegate void OnStateExit(StateTransitionInfo info);
    OnStateExit onStateExit;
    int _currentIndex = 0;
    public void Update()
    {
        // 現在エントリーしているステートの実行
        _transition[_currentIndex]._current.Entry();
        _transition[_currentIndex]._current.Tick();
        _transition[_currentIndex]._current.Exit();
        var trans = _transition[_currentIndex];
        onStateExit(new StateTransitionInfo(trans._current, trans._next));
        // もし条件を満たしたのなら次の遷移先のステートへとぶ
        var work = FindNextEntryPoint(_transition[_currentIndex]._next);
        _currentIndex = (work != -1) ? work : 0; // もし、どこにもいかないのならデフォルトのステートへいく
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
public struct StateTransitionInfo
{
    IAIState _current;
    IAIState _next;
    public StateTransitionInfo(IAIState current, IAIState next)
    {
        _current = current;
        _next = next;
    }
}
public interface IAIState
{
    public void Entry();
    public void Tick();
    public void Exit();
}