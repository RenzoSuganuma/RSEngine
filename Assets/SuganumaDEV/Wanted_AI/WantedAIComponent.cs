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
    WantedAIStateIdle _sIdle = new WantedAIStateIdle();
    /// <summary> ステート：注視 </summary>
    WantedAIStateGaze _sGaze = new WantedAIStateGaze();
    /// <summary> ステート：追跡 </summary>
    WantedAIStateChase _sChase = new WantedAIStateChase();
    /// <summary> ステート：攻撃 </summary>
    WantedAIStateAttack _sAttack = new WantedAIStateAttack();
    /// <summary> ステート：死亡 </summary>
    WantedAIStateDeath _sDeath = new WantedAIStateDeath();

    public void OnStateWasExitted(StateTransitionInfo info)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
