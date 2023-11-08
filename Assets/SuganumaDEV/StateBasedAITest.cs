using RSEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateBasedAITest : MonoBehaviour, IStateMachineUser
{
    // ステートマシン
    StateMachine _smachine = new();
    // 各ステート
    Idle _sidle = new();
    Chase _schase = new();
    Attack _sattack = new();

    public void OnStateWasExitted(StateTransitionInfo info)
    {
        
    }
    private void Awake()
    {
        // ステートの登録 [1]
        _smachine.AddState(_sidle);
        _smachine.AddState(_schase);
        _smachine.AddState(_sattack);
        // ステートの遷移の登録 [2]
        _smachine.AddTransition(_sidle, _schase);
        //  ステートイベントのリスナーの登録 [3]より先に実行
        _smachine.onStateExit += OnStateWasExitted;
        //ステートマシン起動 [3]
        _smachine.Initialize();
        // テスト実行
        _smachine.Update();
    }
    private void FixedUpdate()
    {
        _smachine.UpdateCondition(0, true);
        _smachine.Update();
    }
    private void OnDisable()
    {
        _smachine.onStateExit -= OnStateWasExitted;
    }
}
class Idle : IState
{
    bool _ready2GoNextState = false;
    public void In()
    {
        Debug.Log($"Idle : {nameof(this.In)}");
    }

    public void Out()
    {
        Debug.Log($"Idle : {nameof(this.Out)}");
    }

    public bool ReadyToGoNext()
    {
        return _ready2GoNextState;
    }

    public void SendMessageGotoNext()
    {
        _ready2GoNextState = true;
    }

    public void Tick()
    {
        Debug.Log($"Idle : {nameof(this.Tick)}");
    }
}
class Chase : IState
{
    bool _ready2GoNextState = false;
    public void In()
    {
        Debug.Log($"Chase : {nameof(this.In)}");
    }

    public void Out()
    {
        Debug.Log($"Chase : {nameof(this.Out)}");
    }

    public bool ReadyToGoNext()
    {
        return _ready2GoNextState;
    }

    public void SendMessageGotoNext()
    {
        _ready2GoNextState = true;
    }

    public void Tick()
    {
        Debug.Log($"Chase : {nameof(this.Tick)}");
    }
}
class Attack : IState
{
    bool _ready2GoNextState = false;
    public void In()
    {
    }

    public void Out()
    {
    }

    public bool ReadyToGoNext()
    {
        return _ready2GoNextState;
    }

    public void SendMessageGotoNext()
    {
        _ready2GoNextState = true;
    }

    public void Tick()
    {
    }
}