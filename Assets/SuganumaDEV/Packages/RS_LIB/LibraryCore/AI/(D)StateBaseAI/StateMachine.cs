using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        // a => b, b => c : の部分吟味が必要
        // ステートマシン 
        // もし登録した遷移を満たす条件が合えば遷移する
        /// <summary>  </summary>
        public class StateMachine
        {
            /// <summary> ステートペアの遷移リスト（重複X） </summary>
            /// 重複させない
            HashSet<StatePairedTransition> _htransition = new();

            /// <summary> ステートペアの遷移リスト </summary>
            List<StatePairedTransition> _transition = new();

            /// <summary> 前フレームで実行した遷移の状態を保持している変数 </summary>
            StatePairedTransition _ptransition;

            /// <summary> ステートのリスト（重複X） </summary>
            /// 重複させない
            HashSet<IState> _hstates = new();
            /// <summary> ステートのリスト </summary>

            List<IState> _states = new();

            /// <summary> 現在実行中のステート </summary>
            int _currentTransitionIndex = -1;

            /// <summary> ステートから抜けたときにステートマシン側で呼び出されるイベント </summary>
            /// <param name="info"></param>
            public delegate void OnStateExit(StateTransitionInfo info);

            /// <summary> コールバックリスナーの登録先のデリゲート </summary>
            public event OnStateExit onStateExit;

            #region Machine
            /// <summary> ステートマシン起動時に呼び出す。ステートマシンを起動する </summary>
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
                currentState.Do();
                currentState.Out();
            }
            #endregion

            #region Transition
            /// <summary> 遷移idを指定してそれに対応した条件式を代入 </summary>
            /// <param name="transitionID"></param>
            /// <param name="condition"></param>
            public void UpdateTransitionCondition(int transitionID, bool condition)
            {
                // 直前に行った遷移で整合性を取れたら繊維をする
                if (_ptransition == null) // 履歴がないなら登録
                {
                    var ptransition = _transition[transitionID];
                    _ptransition = new(ptransition.GetState(0), ptransition.GetState(1), transitionID);
                }
                else
                {
                    var tID = _ptransition.GetTransitionId();
                    //if()
                }
                var transition = _transition[transitionID];
                // 条件を満たしたらステートに入った状態を維持する
                if (transition.GetCurrentState() == 0) // まだ遷移していないなら
                {
                    transition.GotoNextCondition(condition);
                }
                else // 遷移が終わり、ステートを抜けたなら
                {
                    List<int> work = new(); // 進める遷移idを格納
                    for (int i = 0; i < _transition.Count; i++)
                    {
                        if (transition.Current == _transition[i].GetState(0))
                        {
                            work.Add(i);
                        }
                    } // 次にとぶ 遷移先が遷移元としてしていされている遷移を探す

                }
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
            #endregion

            #region State
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
            #endregion

            #region States
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
            #endregion
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
            /// <summary> もし条件が満たされたら遷移先ステートへ移ってとどまる。 </summary>
            /// <param name="condition"></param>
            public void GotoNextCondition(bool condition)
            {
                /*|| 条件式を満たしているときにのみ遷移先ステートを実行するため一時無効化 11 / 09 ||*/
                #region 11/09 | 一時無効化処理
                /*
                if (condition)
                {
                    _current = _to;
                }
                else
                {
                    _current = _from;
                }
                */
                #endregion
                /*|| --- ||*/
                // 条件式が真でかつまだ現状のステートが遷移元の時にのみ実行。
                // 一度だけ遷移先に移る。
                _current = (condition && _current == _from) ? _to : _current;
            }
            /// <summary> 現在いるステートを返す </summary>
            /// <returns>0 : (ステートペアの遷移元) 1 : (ステートペア遷移先)</returns>
            public int GetCurrentState()
            {
                return (_current == _from) ? 0 : 1; // from => 0 : to => 1
            }
            /// <summary> 遷移idを返す </summary>
            /// <returns></returns>
            public int GetTransitionId()
            {
                return transitionID;
            }
            /// <summary> 指定されたステートidのステートを返す </summary>
            /// <param name="stateId"></param>
            /// <returns></returns>
            public IState GetState(int stateId)
            {
                return (stateId == 0) ? _from : _to;
            }
        }

        /// <summary> 現状エントリーしているステートペアの遷移のステートの情報を保持する構造体 </summary>
        public struct StateTransitionInfo // ← 修正が必要か要吟味
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
            public void Do();
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