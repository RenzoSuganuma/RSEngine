using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        /// <summary> �l�̂ł����l���̋@�\��񋟂���N���X�B�ړ����i�� </summary>
        public class AILimbComponent : MonoBehaviour
        {
            /// <summary> �w�肵���n�_�ƏI�_���狗��������o���ĖړI�̋����̒l�ɒB������ true ��Ԃ��B </summary>
            /// <param name="strt"></param>
            /// <param name="end"></param>
            /// <param name="limdis"></param>
            /// <param name="limoffset"></param>
            /// <returns></returns>
            bool CheckInsideOfBorder(Vector3 strt, Vector3 end, float limdis, float limoffset) // detect based distance
            {
                float dx = end.x - strt.x;
                float dy = strt.y - end.y;
                float dz = end.z - strt.z;
                float dd = dx * dx + dy * dy + dz * dz;
                float lim = limdis * limdis;
                return dd < lim + limoffset;
            }
            /// <summary> �n���ꂽ���W��ڎw���Đi�� </summary>
            /// <param name="point"></param>
            /// <param name="speed"></param>
            public void MoveToPoint(Vector3 point, float speed)
            {
                var dir = (point - transform.position).normalized;
                dir.y = 0;
                var vel = dir * speed * Time.deltaTime;
                transform.position += vel;
            }
        }
    }
}