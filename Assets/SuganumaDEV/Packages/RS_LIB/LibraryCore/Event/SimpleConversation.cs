using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RSEngine
{
    namespace Event
    {
        public class SimpleConversation : MonoBehaviour
        {
            [SerializeField] Text _convText; // 会話のテキスト表示
            [SerializeField] Image _convPanel; // テキストのBGパネル
            [SerializeField] Button _convStartButton; // ”会話をする”ボタン
            [SerializeField] Button _convNextButton; // ”次へ”ボタン
            [SerializeField] Transform _speakerTransform; // 会話者

            [SerializeField] LayerMask _playerLayer; // プレイヤーレイヤ

            [SerializeField, Range(1f, 5f)] float _conversationRange; // 会話可能距離

            bool _isConversible = false; // 会話スタートしたフラグ

            public void OnStartConversation()
            {
                _convPanel.gameObject.SetActive(true);
                _isConversible = true;
            }

            private void Awake()
            {
                _convStartButton.gameObject.SetActive(false); // みえなくする
                _convPanel.gameObject.SetActive(false); // みえなくする
            }

            private void FixedUpdate()
            {
                bool canSpeak = Physics.CheckSphere(_speakerTransform.position, _conversationRange, _playerLayer); // 会話可能圏内にいるなら
                _convStartButton.gameObject.SetActive(canSpeak); // 可視化
                if (!canSpeak && _isConversible)
                {
                    _convPanel.gameObject.SetActive(false); // みえなくする
                    _isConversible = false;
                }
            }

#if UNITY_EDITOR_64
            private void OnDrawGizmos()
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(_speakerTransform.position, _conversationRange);
            }
#endif
        }
    }
}