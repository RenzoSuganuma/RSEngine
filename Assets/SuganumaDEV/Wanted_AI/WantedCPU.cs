using RSEngine.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Splines;
public class WantedCPU : MonoBehaviour
{
    // ステートマシン
    /// <summary> AIのステートベースな処理をサポートするためのステートマシン </summary>
    StateMachineFoundation _stateMachine;

    // ステート
    /// <summary> デフォルトステート：パトロール </summary>
    WantedAIStateDefault _sDef;
    /// <summary> ステート：注視 </summary>
    WantedAIStateGaze _sGaze;
    /// <summary> ステート：追跡 </summary>
    WantedAIStateChase _sChase;
    /// <summary> ステート：攻撃 </summary>
    WantedAIStateAttack _sAttack;
    /// <summary> ステート：死亡 </summary>
    WantedAIStateDeath _sDeath;

    // 動かすのに必要
    NavMeshAgent _agent;

    // 各レンジ
    [SerializeField, Range(0f, 50f)] float _sightRange;
    [SerializeField, Range(0f, 50f)] float _attackRange;
    // 各レイヤマスク
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] LayerMask _groundLayer;
    // ターゲット
    [SerializeField] Transform _target;
    [SerializeField] int _targetLayerNum;
    // 移動速度
    [SerializeField] float _movespeed;
    // 徘徊経路
    [SerializeField] SplineContainer _patrolingPath;
    // 体力
    [SerializeField] float _health;

    Transform _selfTransform;

    // AIトランジションフラグ
    bool _isInsideSightRange = false; // デフォルトから注視するまでの条件
    bool _isFoundTargetNow = false; // 注視が終わり、プレイヤーとして判定した場合　追跡するかの条件
    bool _isInsideAttackingRange = false; // 追跡をしていて攻撃可能圏内にプレイヤーが入った場合　攻撃するかの条件
    bool _isNoHealthNow = false;　// 死亡をした場合

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _selfTransform = transform;

        // ステートマシン初期化
        _stateMachine = new();

        // 各ステート初期化
        _sDef = new(_patrolingPath, _agent, ref _selfTransform);
        _sGaze = new(_sightRange, 2, transform, _targetLayer, _agent
            , (tTransform) =>
            {
                var dir = (tTransform.position - transform.position).normalized;
                dir.y = 0;
                transform.forward = dir;
            } // On Gazing
            , (tTransform) =>
            {
                var dir = (tTransform.position - transform.position).normalized;
                dir.y = 0;
                transform.forward = dir;
                if (!_isFoundTargetNow) _isFoundTargetNow = true;
            } // On Target Found
            );

        _sChase = new(_sightRange, _targetLayer, transform, _agent);
        _sAttack = new(_attackRange, transform, _targetLayer, _agent
            , () =>
            {
                Debug.Log("攻撃中....");
            }
            );
        _sDeath = new(transform, _agent);

        // 各ステートの登録
        _stateMachine.ResistStates(new List<IState> {
        _sDef,
        _sGaze,
        _sChase,
        _sAttack,});

        _stateMachine.ResisteStateFromAny(_sDeath);

        _stateMachine.ResistTransition(_sDef, _sGaze, "D2G"); // default to gaze id{0}
        _stateMachine.ResistTransition(_sGaze, _sDef, "G2D"); // gaze to default id{1}

        _stateMachine.ResistTransition(_sGaze, _sChase, "G2C"); // gaze to chase id{2}
        _stateMachine.ResistTransition(_sChase, _sGaze, "C2G"); // chase to default id{3}

        _stateMachine.ResistTransition(_sChase, _sAttack, "C2A"); // chase to attack id{4}
        _stateMachine.ResistTransition(_sAttack, _sChase, "A2C"); // attack to chase id{5}

        _stateMachine.ResistTransitionFromAny(_sDeath, "DummyTransition");

        _stateMachine.PopStateMachine();
    }

    private void FixedUpdate()
    {
        // 視野内かの判定
        _isInsideSightRange = Physics.CheckSphere(transform.position, _sightRange, _targetLayer);
        _isInsideAttackingRange = Physics.CheckSphere(transform.position, _attackRange, _targetLayer);
        if (!_isInsideSightRange && _isFoundTargetNow) _isFoundTargetNow = false;
        _isNoHealthNow = _health <= 0;

        // 各ステート更新
        _sDef.UpdateSelf(transform);
        _sGaze.UpdateSelf(transform);
        _sChase.UpdateSelf(transform);
        _sAttack.UpdateSelf(transform);
        _sDeath.UpdateSelf(transform);

        // defalut to gaze
        _stateMachine.UpdateConditionOfTransition("D2G", ref _isInsideSightRange);

        // gaze to deafult
        _stateMachine.UpdateConditionOfTransition("G2D", ref _isInsideSightRange, !true);

        // gaze to chase
        _stateMachine.UpdateConditionOfTransition("G2C", ref _isFoundTargetNow);

        // chase to gaze
        _stateMachine.UpdateConditionOfTransition("C2G", ref _isFoundTargetNow, !true);

        // chase to attack
        _stateMachine.UpdateConditionOfTransition("C2A", ref _isInsideAttackingRange);

        // attack to chase
        _stateMachine.UpdateConditionOfTransition("A2C", ref _isInsideAttackingRange, !true);

        // any state to death
        _stateMachine.UpdateConditionTransitionOfAnyState("DummyTransition", ref _isNoHealthNow);
    }

#if UNITY_EDITOR_64
    private void OnDrawGizmos()
    {
        // 視野
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);

        // 攻撃範囲
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
#endif
}
