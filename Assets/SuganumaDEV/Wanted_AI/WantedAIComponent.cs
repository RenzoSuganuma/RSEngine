using System.Collections.Generic;
using UnityEngine;
using RSEngine.AI.StateMachine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
/// <summary> AI機能を提供するコンポーネント </summary>
public class WantedAIComponent : MonoBehaviour, IStateMachineUser
{
    // ステートマシン
    /// <summary> AIのステートベースな処理をサポートするためのステートマシン </summary>
    StateMachine _sMachine;

    // ステート
    /// <summary> デフォルトステート：アイドル </summary>
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
    [SerializeField] List<Transform> _patrollingPath;

    // AIトランジションフラグ
    bool _isInsideSightRange = false; // デフォルトから注視するまでの条件
    bool _isFoundTargetNow = false; // 注視が終わり、プレイヤーとして判定した場合　追跡するかの条件
    bool _isInsideAttackingRange = false; // 追跡をしていて攻撃可能圏内にプレイヤーが入った場合　攻撃するかの条件
    bool _isNoHealthNow = false;　// 死亡をした場合

    public void OnStateWasExitted(StateTransitionInfo info)
    {

    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        // ステートマシン初期化
        _sMachine = new StateMachine();

        // 各ステート初期化
        _sDef = new(_patrollingPath.ToArray(), _agent);
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
        _sAttack = new();
        _sDeath = new();

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
        _sMachine.AddTransition(_sChase, _sGaze); // chase to default id{3}

        //　追跡から攻撃
        _sMachine.AddTransition(_sChase, _sAttack); // chase to attack id{4}
        _sMachine.AddTransition(_sAttack, _sChase); // attack to chase id{5}

        //// 死亡ステート
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
        // 視野内かの判定
        _isInsideSightRange = Physics.CheckSphere(transform.position, _sightRange, _targetLayer);
        _isInsideAttackingRange = Physics.CheckSphere(transform.position, _attackRange, _targetLayer);
        if (!_isInsideSightRange && _isFoundTargetNow) _isFoundTargetNow = false;

        // 各ステート更新
        _sDef.Update(transform);
        _sGaze.Update(transform);
        _sChase.Update(transform);

        // defalut to gaze
        _sMachine.UpdateTransitionCondition(0, _isInsideSightRange);

        // gaze to deafult
        _sMachine.UpdateTransitionCondition(1, !_isInsideSightRange);

        // gaze to chase
        _sMachine.UpdateTransitionCondition(2, _isFoundTargetNow);

        // chase to gaze
        _sMachine.UpdateTransitionCondition(3, !_isFoundTargetNow);

        // chase to attack
        _sMachine.UpdateTransitionCondition(4, _isInsideAttackingRange);

        // attack to chase
        _sMachine.UpdateTransitionCondition(5, !_isInsideAttackingRange);

        //// any state to death
        _sMachine.UpdateTransitionCondition(6, _isNoHealthNow);
        _sMachine.UpdateTransitionCondition(7, _isNoHealthNow);
        _sMachine.UpdateTransitionCondition(8, _isNoHealthNow);
        _sMachine.UpdateTransitionCondition(9, _isNoHealthNow);

        // update statemachine
        _sMachine.Update();
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