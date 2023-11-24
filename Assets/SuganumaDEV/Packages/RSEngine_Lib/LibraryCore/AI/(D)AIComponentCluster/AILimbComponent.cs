// 管理者 菅沼
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        namespace Dev
        {
            /// <summary> 人体でいう四肢の機能を提供するクラス。移動を司る </summary>
            public class AILimbComponent : MonoBehaviour
            {
                /// <summary> 渡された座標を目指して進む </summary>
                /// <param name="point"></param>
                /// <param name="speed"></param>
                public void MoveToPoint(Vector3 point, float speed)
                {
                    var dir = (point - transform.position).normalized;
                    var vel = dir * speed * Time.deltaTime;
                    transform.position += vel;
                }
            }
        }
    }
}