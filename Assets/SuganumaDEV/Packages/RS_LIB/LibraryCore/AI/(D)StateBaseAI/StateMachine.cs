using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        // ステートマシン 
        // もし登録した遷移を満たす条件が合えば遷移する
        /// <summary>  </summary>
        public class StateMachine
        {
            /// <summary> ステートペアの遷移リスト（重複X） </summary>
            /// 重複させない
            HashSet<StatePairedTransition> _htransition = new();
            List<StatePairedTransition> _transition = new();

            /// <summary> ステートのリスト（重複X） </summary>
            /// 重複させない
            HashSet<IState> _hstates = new();
            List<IState> _states = new();

            /// <summary> 現在実行中のステート </summary>
            int _currentTransitionIndex = -1;

            /// <summary> ステートから抜けたときにステートマシン側で呼び出されるイベント </summary>
            /// <param name="info"></param>
            public delegate void OnStateExit(StateTransitionInfo info);

            /// <summary> コールバックリスナーの登録先のデリゲート </summary>
            public event OnStateExit onStateExit;

            /// <summary> ステートマシン起動時に呼び出す </summary>
            public void Initialize()
            {
                _transition = _htransition.ToList();
                _states = _hstates.ToList();
                _currentTransitionIndex = 0;
            }

            /// <summary> 毎フレーム呼び出すメソッド </summary>
            public void Update()
            {
                // ステートの実行
                var currentState = _transition[_currentTransitionIndex].Current;
                currentState.In();
                currentState.Tick();
                currentState.Out();
            }

            /// <summary> 遷移IDを指定してそれに対応した条件式を代入 </summary>
            /// <param name="transitionId"></param>
            /// <param name="condition"></param>
            public void UpdateCondition(int transitionId, bool condition)
            {
                _transition[transitionId].UpdateCondition(condition);
            }

            /// <summary> ステートの登録をする </summary>
            /// <param name="state"></param>
            public void AddState(IState state)
            {
                _hstates.Add(state);
            }

            /// <summary> ステートの登録解除をする </summary>
            /// <param name="state"></param>
            public void RemoveState(IState state)
            {
                _hstates.Remove(state);
            }

            /// <summary> リスト形式で渡されたステートを登録する </summary>
            /// <param name="states"></param>
            public void AddStates(List<IState> states)
            {
                foreach (IState state in states)
                {
                    _hstates.Add(state);
                }
            }

            public void ClearAllStates()
            {
                _hstates.Clear();
            }

            /// <summary> 遷移元と遷移先の情報を保持するステートペアを登録する。 </summary>
            /// <param name="from"></param>
            /// <param name="to"></param>
            public void AddTransition(IState from, IState to)
            {
                _htransition.Add(new StatePairedTransition(from, to, _htransition.Count /* id => 0 ~ */));
            }

            /// <summary> すべてのステートペアの遷移をクリアする </summary>
            public void ClearTransition()
            {
                _htransition.Clear();
            }
        }

        // 遷移元と遷移先
        /// <summary> 遷移元と遷移先の情報を保持するクラス </summary>
        /// <typeparam name="Tcurrent"> 遷移元 </typeparam>
        /// <typeparam name="Tnext"> 遷移先 </typeparam>
        public class StatePairedTransition// ← 遷移の矢印に当たる 修正いらない
        {
            public StatePairedTransition(IState from, IState to, int transitionID)
            {
                _from = from;
                _to = to;
                _current = _from;
                this.transitionID = transitionID;
            }
            IState _from; // id = 0
            IState _to; // id = 1
            IState _current; // id => current State
            public IState Current => _current;
            int transitionID; // transition id non duplication
            /// <summary> 毎フレーム呼び出す。条件を更新する。 </summary>
            /// <param name="condition"></param>
            public void UpdateCondition(bool condition)
            {
                if (condition)
                {
                    _current = _to;
                }
                else
                {
                    _current = _from;
                }
            }
            /// <summary> 現在いるステートを返す </summary>
            /// <returns></returns>
            public int GetCurrentState()
            {
                return (_current == _from) ? 0 : 1; // from => 0 : to => 1
            }
            /// <summary> 遷移ＩＤを返す </summary>
            /// <returns></returns>
            public int GetTransitionId()
            {
                return transitionID;
            }
        }

        /// <summary> 現状エントリーしているステートペアの遷移のステートの情報を保持する構造体 </summary>
        public struct StateTransitionInfo // ← わからない
        {
            public IState _from;
            public IState _to;
            public int _id;
            public StateTransitionInfo(IState from, IState to, int id)
            {
                _from = from;
                _to = to;
                _id = id;
            }
        }

        /// <summary> ステートマシンが扱う遷移リストに登録するクラスが継承すべきインターフェイス </summary>
        public interface IState // ← 修正いらない
        {
            /// <summary> ステート突入時に呼び出される </summary>
            public void In();
            /// <summary> ステート通過時に呼び出される </summary>
            public void Tick();
            /// <summary> ステート脱出時に呼び出される </summary>
            public void Out();
            /// <summary> 次のステートへ遷移しても良いのかの判定 </summary>
            /// <returns></returns>
            public bool ReadyToGoNext();
            /// <summary> 強制的に次ステートへ行く </summary>
            public void SendMessageGotoNext();
        }

        /// <summary> ステートマシン利用部クラスが継承する </summary>
        public interface IStateMachineUser // ← 修正いらない
        {
            /// <summary> ステートの In(),Tick(),Out() 呼び出し直後に発火するイベントのリスナー </summary>
            /// <param name="info"></param>
            public void OnStateWasExitted(StateTransitionInfo info);
        }
    }
}