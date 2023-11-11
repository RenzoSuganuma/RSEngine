using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSEngine.AI.StateMachine;

/// <summary> AI機能を提供するコンポーネント </summary>
public class WantedAIComponent : MonoBehaviour, IStateMachineUser
{
    // ステートマシン
    /// <summary> AIのステートベースな処理をサポートするためのステートマシン </summary>
    StateMachine _sMachine = new StateMachine();

    // ステート
    /// <summary> デフォルトステート：アイドル </summary>
    WantedAIStateDefault _sDef = new WantedAIStateDefault();
    /// <summary> ステート：注視 </summary>
    WantedAIStateGaze _sGaze = new WantedAIStateGaze();
    /// <summary> ステート：追跡 </summary>
    WantedAIStateChase _sChase = new WantedAIStateChase();
    /// <summary> ステート：攻撃 </summary>
    WantedAIStateAttack _sAttack = new WantedAIStateAttack();
    /// <summary> ステート：死亡 </summary>
    WantedAIStateDeath _sDeath = new WantedAIStateDeath();

    // AIステート判定
    bool _isDefalultNow = false;
    bool _isGazing = false;
    bool _isChasing = false;
    bool _isAttacking = false;
    bool _isDeathNow = false;

    // AIトランジションフラグ
    bool _isInsideSightRange = false; // デフォルトから注視するまでの条件
    bool _isFoundTargetNow = false; // 注視が終わり、プレイヤーとして判定した場合　追跡するかの条件
    bool _isInsideAttackingRange = false; // 追跡をしていて攻撃可能圏内にプレイヤーが入った場合　攻撃するかの条件
    bool _isNoHealthNow = false;　// 死亡をした場合

    public void OnStateWasExitted(StateTransitionInfo info)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        // イベントリスナー登録
        _sMachine.onStateExit += OnStateWasExitted;
        // 各ステートの登録
        _sMachine.AddStates(new List<IState> {
        _sDef,
        _sGaze,
        _sChase,
        _sAttack,
        _sDeath});
        // 通常から注視
        _sMachine.AddTransition(_sDef, _sGaze); // default to gaze id{0}
        _sMachine.AddTransition(_sGaze, _sDef); // gaze to default id{1}
        // 注視から追跡
        _sMachine.AddTransition(_sGaze, _sChase); // gaze to chase id{2}
        _sMachine.AddTransition(_sGaze, _sChase); // chase to gaze id{3}
        //　追跡から攻撃
        _sMachine.AddTransition(_sChase, _sAttack); // chase to attack id{4}
        _sMachine.AddTransition(_sAttack, _sChase); // attack to chase id{5}
        // 死亡ステート
        _sMachine.AddTransition(_sDef, _sDeath); // id{6}
        _sMachine.AddTransition(_sGaze, _sDeath); // id{7}
        _sMachine.AddTransition(_sChase, _sDeath); // id{8}
        _sMachine.AddTransition(_sAttack, _sDeath); // id{9}
        // ステートマシン起動
        _sMachine.Initialize();
    }

    private void OnDisable()
    {
        //　リスナー登録解除
        _sMachine.onStateExit -= OnStateWasExitted;
    }

    private void FixedUpdate()
    {
        // defalut to gaze
        _sMachine.UpdateTransitionCondition(0, _isInsideSightRange);
        _sMachine.UpdateTransitionCondition(1, !_isInsideSightRange);
        // gaze to chase
        _sMachine.UpdateTransitionCondition(2, _isFoundTargetNow);
        _sMachine.UpdateTransitionCondition(3, !_isFoundTargetNow);
        // chase to attack
        _sMachine.UpdateTransitionCondition(4, _isInsideAttackingRange);
        _sMachine.UpdateTransitionCondition(5, !_isInsideAttackingRange);
        // any state to death
        _sMachine.UpdateTransitionCondition(6, _isNoHealthNow);
        _sMachine.UpdateTransitionCondition(7, _isNoHealthNow);
        _sMachine.UpdateTransitionCondition(8, _isNoHealthNow);
        _sMachine.UpdateTransitionCondition(9, _isNoHealthNow);
    }
}