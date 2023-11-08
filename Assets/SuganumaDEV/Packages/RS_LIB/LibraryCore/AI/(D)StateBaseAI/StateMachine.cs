using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        // ステートマシン 
        // もし登録した遷移を満たす条件が合えば遷移する
        public class StateMachine
        {
            List<StateTransition<IAIState, IAIState>> _transition = new();
            public delegate void OnStateExit(StateTransitionInfo info);
            public event OnStateExit onStateExit;
            bool _isCurrentStateNow = true;
            bool _nextStateDone = false; // 現在の遷移ペアの次のステートが終わったならtrue
            int _currentTransitionIndex = 0; // 現在の遷移のリストからのインデックス
            public void Update()
            {
                // 現在エントリーしているステートの実行
                if (_isCurrentStateNow) // ステートペア遷移元なら
                {
                    _transition[_currentTransitionIndex]._current.In();
                    _transition[_currentTransitionIndex]._current.Tick();
                    _transition[_currentTransitionIndex]._current.Out();
                    // current の処理が終わって次に遷移できるか判定
                    _isCurrentStateNow = !_transition[_currentTransitionIndex]._current.ReadyToGoNext();
                }
                else
                {
                    _transition[_currentTransitionIndex]._next.In();
                    _transition[_currentTransitionIndex]._next.Tick();
                    _transition[_currentTransitionIndex]._next.Out();
                    // current の処理が終わって次に遷移できるか判定
                    var done = _transition[_currentTransitionIndex]._current.ReadyToGoNext();
                    // 遷移ペア次ステート
                    if (!_nextStateDone && done) { _nextStateDone = true; }
                    // ステート選択を初期値にもどす
                    _isCurrentStateNow = done; 
                }
                // ステートペアのどちらかから抜けたらイベントの発火
                var trans = _transition[_currentTransitionIndex];
                onStateExit(new StateTransitionInfo(trans._current, trans._next));
                // 次の遷移ペアへ遷移
                if (_nextStateDone)
                {
                    GotoNextStatePair();
                    _nextStateDone = false;
                }
            }
            public void AddTransition(IAIState current, IAIState next)
            {
                _transition.Add(new StateTransition<IAIState, IAIState>(current, next));
            }
            public void ClearTransition()
            {
                _transition.Clear();
            }
            public void GotoNextStatePair()
            {
                var work = FindNextFirstEntryPoint(_transition[_currentTransitionIndex]._next);
                if (work != -1)
                {
                    _currentTransitionIndex = work;// もし、どこにもいかないのならそのステートにとどまる
                }
            }
            // 渡された StateTransition.next のステートがStateTransition.current として登録されている遷移リストのインデックスを返す
            // 複数の遷移に対応していないので追々対応させるように
            int FindNextFirstEntryPoint(IAIState next)
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
            public bool ReadyToGoNext();
            public void SendMessageGotoNext();
        }
        public interface IStateMachineUser
        {
            public void OnStateWasExitted(StateTransitionInfo info);
        }
    }
}