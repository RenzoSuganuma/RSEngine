using System;
using System.Collections.Generic;
using UnityEngine;

namespace SgLibUnite.Animations
{
    [RequireComponent(typeof(Animator))]
    /// <summary>
    /// 登録名に対応した値を毎フレーム自動的にAnimatorクラスへあらかじめ登録しているパラメータを更新する
    /// </summary>
    public class AnimAutomator : MonoBehaviour
    {
        enum UpdateMode
        {
            Update,
            FixedUpdate
        }

        [SerializeField] private UpdateMode mode;

        private Animator _anim;

        private Dictionary<string, Tuple<AnimatorControllerParameterType, System.Object>> _parameterDic =
            new Dictionary<string, Tuple<AnimatorControllerParameterType, object>>();

        public void SubscribeAnimatorControllerParamValue(string paramName,
            Tuple<AnimatorControllerParameterType, System.Object> typeAndValue)
        {
            this._parameterDic.Add(paramName, typeAndValue);
        }

        void TaskToSetParamValue(AnimatorControllerParameter[] param)
        {
            foreach (var parameter in param)
            {
                var name = parameter.name;
                var t = parameter.type;
                var target = _parameterDic[name]; // ここでBUGでたならTryGetValueメソッドを使おう
                if (t == target.Item1)
                {
                    switch (t)
                    {
                        case AnimatorControllerParameterType.Bool:
                            _anim.SetBool(name, (bool)target.Item2);
                            break;
                        case AnimatorControllerParameterType.Float:
                            _anim.SetFloat(name, (float)target.Item2);
                            break;
                        case AnimatorControllerParameterType.Int:
                            _anim.SetInteger(name, (int)target.Item2);
                            break;
                    }
                }
            }
        }

        private void Start()
        {
            _anim = this.GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (mode == UpdateMode.FixedUpdate)
            {
                var p = _anim.parameters;
                TaskToSetParamValue(p);
            }
        }

        private void Update()
        {
            if (mode == UpdateMode.Update)
            {
                var p = _anim.parameters;
                TaskToSetParamValue(p);
            }
        }
    }
}