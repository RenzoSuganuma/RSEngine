using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationComponent : MonoBehaviour
{
    [SerializeField] Text _convText; // 会話のテキスト表示
    [SerializeField] Image _convPanel; // テキストのBGパネル
    [SerializeField] Button _convStartButton; // ”会話をする”ボタン
    [SerializeField] Button _convNextButton; // ”次へ”ボタン

    [SerializeField] LayerMask _playerLayer; // プレイヤーレイヤ

    [SerializeField, Range(1f, 5f)] float _conversationRange; // 会話可能距離

    private void Awake()
    {
        _convStartButton.gameObject.SetActive(false); // みえなくする
        _convNextButton.gameObject.SetActive(false); // みえなくする
    }

    private void FixedUpdate()
    {
        if (Physics.CheckSphere(transform.position, _conversationRange, _playerLayer))
        {
            _convStartButton.gameObject.SetActive(true); // 可視化
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