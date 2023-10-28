using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        /// <summary> 人体でいう四肢の機能を提供するクラス。移動を司る </summary>
        public class AILimbComponent : MonoBehaviour
        {
            /// <summary> 指定した始点と終点から距離を割り出して目的の距離の値に達したら true を返す。 </summary>
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
            /// <summary> 渡された座標を目指して進む </summary>
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