using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        /// <summary> 人体でいう脳に当たる機能を提供する </summary>
        [RequireComponent(typeof(AILimbComponent))]
        public class AIBrainComponent : MonoBehaviour
        {
            [SerializeField, Header("Targets")] List<Transform> _targets = new();
            int _targetIndex = 0;
            [SerializeField, Header("Target Layer Number")] int _targetLayerNum;
            [SerializeField, Header("MoveSpeed"), Range(1, 10)] float _moveSpeed;
            [SerializeField, Header("Path")] List<Vector3> _path;
            int _pathIndex = 0;
            AILimbComponent _aiLimb;
            /// <summary> たどる道筋を始点と終点を指定して割り出す </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <param name="pointCount"></param>
            /// <returns></returns>
            Vector3[] DetectPath(Vector3 start, Vector3 end, int pointCount)
            {
                start.y = 0; end.y = 0; // temporary format
                var dir = end - start;
                var ddir = dir / (pointCount - 1);
                Vector3[] path = new Vector3[pointCount];
                path[0] = start;
                path[pointCount - 1] = end;
                if (pointCount > 2)
                {
                    for (int i = 1; i < pointCount - 1; i++)
                    {
                        path[i] = path[i - 1] + ddir;
                    }
                }
                return path;
            }
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
            /// <summary> レベルにターゲットが居るかの検索をかけたうえでターゲットをリターンする </summary>
            /// <returns></returns>
            (bool condition, List<Transform> targets) IsTargetInLevel()
            {
                var list = GameObject.FindObjectsOfType<Transform>().Where(x => x.gameObject.layer == _targetLayerNum).ToList();
                return (list.Count > 0, list);
            }
            void SetPathToTarget((bool condition, List<Transform> targets) result, int targetIndex, int subDivide)
            {
                _targets = (result.condition) ? result.targets : new();
                if (_targets.Count > 0)
                    _path = DetectPath(transform.position, _targets[targetIndex].position, subDivide).ToList();
            }
            private void Start()
            {
                _aiLimb = GetComponent<AILimbComponent>();
                SetPathToTarget(IsTargetInLevel(), _targetIndex, 5);
            }
            private void FixedUpdate()
            {
                _aiLimb.MoveToPoint(_path[_pathIndex], _moveSpeed);
                var result = CheckInsideOfBorder(transform.position, _path[_pathIndex], 1, 1);
                if (result) _pathIndex = (_pathIndex + 1 < _path.Count) ? _pathIndex + 1 : _pathIndex;
            }
            private void OnDrawGizmos()
            {
                // visible path
                Gizmos.color = Color.black;
                Gizmos.DrawLineStrip(_path.ToArray(), true);
                foreach (var path in _path)
                {
                    Gizmos.DrawWireSphere(path, .5f);
                }
            }
        }
    }
}