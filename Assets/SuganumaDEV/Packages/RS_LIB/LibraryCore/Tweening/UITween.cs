using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace RSEngine
{
    namespace Tweening
    {
        enum UIEasingMode
        {
            EaseInQuad,
            EaseOutQuad,
            EaseInOutQuad,
            EaseInCubic,
            EaseOutCubic,
            EaseInOutCubic,
            EaseInQuart,
            EaseOutQuart,
            EaseInOutQuart,
            EaseInQuint,
            EaseOutQuint,
            EaseInOutQuint,
            EaseInExpo,
            EaseOutExpo,
            EaseInOutExpo,
            EaseInCirc,
            EaseOutCirc,
            EaseInOutCirc,
            EaseInBack,
            EaseOutBack,
            EaseInOutBack,
            EaseInElastic,
            EaseOutElastic,
            EaseInOutElastic,
            EaseInBounce,
            EaseOutBounce,
            EaseInOutBounce,
        }

        public class UITween : MonoBehaviour
        {
            #region Process Core
            // 移動してくる画像
            [SerializeField] Image _movingImage;
            // ゴールの画面座標
            [SerializeField] RectTransform _goalRect;
            // スタートの画面座標
            [SerializeField] RectTransform _startRect;
            // デュレーション
            [SerializeField] float _duration;
            // Tweeningが完了したときのイベント
            [SerializeField] UnityEvent _onTweeningEnd;
            // アニメーションしているかのフラグ
            bool _bIsAnimating = false;
            public bool IsTweening => _bIsAnimating;
            float _elapsedTime = 0;

            private void FixedUpdate()
            {
                if (_bIsAnimating)
                {
                    Debug.Log("Tweening");
                    _elapsedTime += Time.deltaTime / _duration;
                    float xPos = (1 - easeOutBounce(_elapsedTime)) * _startRect.position.x + easeOutBounce(_elapsedTime) * _goalRect.position.x;
                    float yPos = (1 - easeOutBounce(_elapsedTime)) * _startRect.position.y + easeOutBounce(_elapsedTime) * _goalRect.position.y;
                    _movingImage.rectTransform.position = new Vector3(xPos, yPos, 0);
                    if (_elapsedTime > 1f)
                    {
                        _elapsedTime = 0f;
                        _bIsAnimating = false;
                        _onTweeningEnd?.Invoke();
                    }
                }
            }

            public void StartTween()
            {
                if (_bIsAnimating) { return; }
                else
                {
                    _bIsAnimating = true;
                }
            }

            #endregion
            #region EasingFormulas
            float easeInSine(float t)
            {
                return 1 - Mathf.Cos((t * Mathf.PI) / 2.0f);
            }
            float easeOutSine(float t)
            {
                return Mathf.Sin((t * Mathf.PI) / 2.0f);
            }
            float easeInOutSine(float t)
            {
                return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
            }

            float easeInQuad(float t)
            {
                return t * t;
            }
            float easeOutQuad(float t)
            {
                return 1 - (1 - t) * (1 - t);
            }

            float easeInOutElastic(float t)
            {
                float c5 = (2 * Mathf.PI) / 4.5f;
                return t == 0
                        ? 0 : t == 1
                        ? 1 : t < 0.5
                        ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2
                        : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
            }

            float easeOutQuint(float t)
            {
                return 1 - Mathf.Pow(1 - t, 5);
            }

            float easeOutBounce(float t)
            {
                float n1 = 7.5625f;
                float d1 = 2.75f;

                if (t < 1f / d1)
                {
                    return n1 * t * t;
                }
                else if (t < 2f / d1)
                {
                    return n1 * (t -= 1.5f / d1) * t + 0.75f;
                }
                else if (t < 2.5f / d1)
                {
                    return n1 * (t -= 2.25f / d1) * t + 0.9375f;
                }
                else
                {
                    return n1 * (t -= 2.625f / d1) * t + 0.984375f;
                }
            }
            #endregion
        }
    }
}