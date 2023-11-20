using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RSEngine
{
    namespace StateMachine
    {
        public partial class StateMachineOrigin : MonoBehaviour
        {
            // ステート
            HashSet<IState> _states = new HashSet<IState>();
            // トランジション
            HashSet<StateMachineTransition> _transitions = new HashSet<StateMachineTransition>();
            // 現在突入しているステート
            IState _currentPlayingState;
            // 現在突入しているトランジション名
            string _currentTransitionName;

            #region 登録処理
            public void ResistState(IState state)
            {
                _states.Add(state);
            }

            // レジストするたびに終点ステートが視点ステートとして割り当てられているステート情報を何かしらのデータ構造のデータを保持する
            public void ResistTransition(IState from, IState to, string name)
            {
                var tmp = new StateMachineTransition(from, to, name);
                _transitions.Add(tmp);
            }
            #endregion

            #region 更新処理
            public void UpdateConditionOfTransition(string name, bool condition2transist)
            {
                foreach (var t in _transitions)
                {
                    if (t.Name == name)
                    {
                        // 遷移する場合 // * 条件を満たしているなら前トランジションを無視してしまうのでその判定処理をはさむこと *
                        if (condition2transist)
                        {
                            _currentPlayingState.Exit();
                            _currentPlayingState = t.STo;
                            _currentPlayingState.Entry();
                            _currentTransitionName = name;
                        }
                        else
                        {
                            _currentPlayingState.Update();
                        }
                    }
                }
            }
            #endregion
        }
        // 各トランジションは名前を割り当てている
        public partial class StateMachineTransition
        {
            IState _from;
            public IState SFrom => _from;
            IState _to;
            public IState STo => _to;
            string _name;
            public string Name => _name;
            public StateMachineTransition(IState from, IState to, string name)
            {
                _from = from;
                _to = to;
                _name = name;
            }
        }

        public interface IState
        {
            public void Entry();
            public void Update();
            public void Exit();
        }

        #region ステートマシン、利用部構想
        // イニシャライズ処理
        // ステートマシンインスタンス化ステートの登録
        // トランジションの登録
        // ステートマシンの更新

        // 毎フレーム処理
        // トランジションの状態の更新

        //開発面
        // partialclassとして宣言して分業の難易度を下げる
        #endregion
    }
}