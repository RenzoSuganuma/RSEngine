using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSEngine.AI.StateMachine;

/// <summary> AI�@�\��񋟂���R���|�[�l���g </summary>
public class WantedAIComponent : MonoBehaviour, IStateMachineUser
{
    // �X�e�[�g�}�V��
    /// <summary> AI�̃X�e�[�g�x�[�X�ȏ������T�|�[�g���邽�߂̃X�e�[�g�}�V�� </summary>
    StateMachine _sMachine = new StateMachine();
    // �X�e�[�g
    /// <summary> �f�t�H���g�X�e�[�g�F�A�C�h�� </summary>
    WantedAIStateIdle _sIdle = new WantedAIStateIdle();
    /// <summary> �X�e�[�g�F���� </summary>
    WantedAIStateGaze _sGaze = new WantedAIStateGaze();
    /// <summary> �X�e�[�g�F�ǐ� </summary>
    WantedAIStateChase _sChase = new WantedAIStateChase();
    /// <summary> �X�e�[�g�F�U�� </summary>
    WantedAIStateAttack _sAttack = new WantedAIStateAttack();
    /// <summary> �X�e�[�g�F���S </summary>
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
