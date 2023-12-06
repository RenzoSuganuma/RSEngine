// 作成者、担当：菅沼
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSEngine.Tweening;
using UnityEditor;
/// <summary> SNSのポップアップのようなイベントの発火を時間経過でランダムにする </summary>
[RequireComponent(typeof(UITween))]
public class SNSPopUpEvent : MonoBehaviour
{
    [SerializeField,
        Header("SNSのポップアップ通知をする間隔")]
    float _eventPopInterval;

    UITween _uiTween;

    float _elapsedTime;

    bool _isCountingTimeForDestoy;

    public void OnPopUpEventEnd()
    {
        _isCountingTimeForDestoy = true;
    }

    public void CallStartTween()
    {
        _uiTween.StartTween();
    }

    private void Start()
    {
        _uiTween = GetComponent<UITween>();
    }

    private void FixedUpdate()
    {
        if (_elapsedTime > _eventPopInterval)
        {
            CallStartTween();
        }
        else
        {
            _elapsedTime += Time.deltaTime;
        }
        if (_isCountingTimeForDestoy)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _eventPopInterval * 2)
                Destroy(gameObject);
        }
    }
}