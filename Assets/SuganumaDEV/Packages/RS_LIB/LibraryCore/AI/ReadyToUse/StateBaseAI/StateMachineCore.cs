using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RSEngine
{
    namespace StateMachine
    {
        public partial class StateMachineCore
        {
            // ステート
            HashSet<IState> _states = new HashSet<IState>();
            // トランジション
            HashSet<StateMachineTransition> _transitions = new HashSet<StateMachineTransition>();
            // 現在突入しているステート
            IState _currentPlayingState;
            // 現在突入しているトランジション名
            string _currentTransitionName;
            // ステートマシンが一時停止中かのフラグ
            bool _bIsPausing;

            #region 登録処理
            public void ResistState(IState state)
            {
                _states.Add(state);
                if (_currentPlayingState == null) { _currentPlayingState = state; }
            }

            // レジストするたびに終点ステートが視点ステートとして割り当てられているステート情報を何かしらのデータ構造のデータを保持する
            public void ResistTransition(IState from, IState to, string name)
            {
                var tmp = new StateMachineTransition(from, to, name);
                _transitions.Add(tmp);
            }
            #endregion

            #region 更新処理
            public void UpdateConditionOfTransition(string name, ref bool condition2transist)
            {
                if (_bIsPausing) return; // もし一時停止中なら更新処理はしない。
                foreach (var t in _transitions)
                {
                    // 遷移する場合 // * 条件を満たしているなら前トランジションを無視してしまうのでその判定処理をはさむこと *
                    // もし遷移条件を満たしていて遷移名が一致するなら
                    if (condition2transist && t.Name == name) 
                    {
                        if (t.SFrom == _currentPlayingState) // 現在左ステートなら
                        {
                            _currentPlayingState.Exit(); // 右ステートへの遷移条件を満たしたので抜ける
                            condition2transist = false; // 遷移条件を初期化(falseに)
                            _currentPlayingState = t.STo; // 現在のステートを右ステートに更新、遷移はそのまま
                            _currentPlayingState.Entry(); // 現在のステートの初回起動処理を呼ぶ
                            _currentTransitionName = name; // 現在の遷移ネームを更新
                        }
                    }
                    // 遷移の条件を満たしてはいないが、遷移ネームが一致（更新されていないなら）現在のステートの更新処理を呼ぶ
                    else if (t.Name == name)
                    {
                        _currentPlayingState.Update();
                    }
                } // 全遷移を検索。
            }
            #endregion

            #region 起動処理
            public void PopStateMachine()
            {
                _bIsPausing = false;
                _currentPlayingState.Entry();
            }
            #endregion

            #region 一時停止処理
            public void PushStateMachine()
            {
                _bIsPausing = true;
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
        // partial classとして宣言して分業の難易度を下げる
        #endregion
    }
}