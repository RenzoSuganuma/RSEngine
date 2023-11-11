using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationComponent : MonoBehaviour
{
    [SerializeField] Text _convText; // ��b�̃e�L�X�g�\��
    [SerializeField] Image _convPanel; // �e�L�X�g��BG�p�l��
    [SerializeField] Button _convStartButton; // �h��b������h�{�^��
    [SerializeField] Button _convNextButton; // �h���ցh�{�^��

    [SerializeField] LayerMask _playerLayer; // �v���C���[���C��

    [SerializeField, Range(1f, 5f)] float _conversationRange; // ��b�\����

    private void Awake()
    {
        _convStartButton.gameObject.SetActive(false); // �݂��Ȃ�����
        _convNextButton.gameObject.SetActive(false); // �݂��Ȃ�����
    }

    private void FixedUpdate()
    {
        if (Physics.CheckSphere(transform.position, _conversationRange, _playerLayer))
        {
            _convStartButton.gameObject.SetActive(true); // ����
        }
    }

#if UNITY_EDITOR_64
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _conversationRange);
    }
#endif
}