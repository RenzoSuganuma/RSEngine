using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace RSEngine
{
    namespace Tweening
    {
        public class UITween : MonoBehaviour
        {
            // 移動してくる画像
            [SerializeField] Image _movingImage;
            // ゴールの画面座標
            [SerializeField] RectTransform _goalRect;
            // スタートの画面座標
            [SerializeField] RectTransform _startRect;
            // デュレーション
            [SerializeField] float _duration;
            // アニメーションしているかのフラグ
            bool _bIsAnimating = false;
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
        }
    }
}