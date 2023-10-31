using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        /// <summary> 人体でいう脳に当たる機能を提供する。各処理に必要な思考を司る </summary>
        [RequireComponent(typeof(AILimbComponent))]
        public class AIBrainComponent : MonoBehaviour
        {
            /// <summary> 追跡対象のターゲットリスト </summary>
            [SerializeField, Header("Targets")] List<Transform> _targets = new();
            /// <summary> ターゲットリストのインデックス </summary>
            int _targetIndex = 0;
            /// <summary> ターゲットのレイヤ番号 </summary>
            [SerializeField, Header("Target Layer Number")] int _targetLayerNum;
            /// <summary> 移動速度 </summary>
            [SerializeField, Header("MoveSpeed"), Range(1, 10)] float _moveSpeed;
            /// <summary> 現在選択しているターゲットまでの道筋。分岐点のリスト </summary>
            [SerializeField, Header("Path")] List<Vector3> _path;
            /// <summary> 障害物のレイヤマスク </summary>
            [SerializeField, Header("Obstacle For AI Layer")] LayerMask _obstacleLayer;
            /// <summary> 分岐点の障害物検知半径 </summary>
            [SerializeField, Header("Path Point Obstacle Avoidance")] float _pathAvoidanceRad;
            /// <summary> 歩行可能な面のレイヤマスク </summary>
            [SerializeField, Header("Walkable Layer")] LayerMask _walkableLayerMask;
            /// <summary> 道筋の分岐点のインデックス </summary>
            int _pathIndex = 0;
            /// <summary> AI四肢コンポーネント </summary>
            AILimbComponent _aiLimb;

            /// <summary> たどる道筋を始点と終点を指定して割り出す </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <param name="pointCount"> 始点終点含めた中間座標点 </param>
            /// <returns></returns>
            Vector3[] DetectPath(Vector3 start, Vector3 end, int pointCount) // 0 ~ pointcount - 1
            {
                #region Make Points:パスの頂点を作成
                start.y = 0; end.y = 0; // <- temporary format,
                                        // guratitudelly set using
                                        // user definded walkable mesh information.
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
                } // make point
                #endregion

                #region Detect Obstacles:障害物の有無を検知、退避
                for (int i = 0; i < pointCount; i++)
                {
                    if (Physics.CheckSphere(path[i], _pathAvoidanceRad, _obstacleLayer))
                    {
                        var cols = Physics.OverlapSphere(path[i], _pathAvoidanceRad, _obstacleLayer);
                        var vec = -(cols[0].transform.position - path[i]);
                        vec.y = 0; // temporary formatting 
                        path[i] += vec;
                        path[(i + 1 < pointCount - 1) ? i + 1 : i] += vec;
                    } // if is there obstacles 

                    if (Physics.CheckSphere(path[i], _pathAvoidanceRad, _walkableLayerMask))
                    {
                        while (Physics.CheckSphere(path[i], _pathAvoidanceRad, _walkableLayerMask))
                        {
                            path[i].y += _pathAvoidanceRad * Time.deltaTime;
                        }
                    }
                } // check each point's near in obstacles
                #endregion

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

            /// <summary> ターゲットまでの道筋を検出する </summary>
            /// <param name="result"></param>
            /// <param name="targetIndex"></param>
            /// <param name="subDivide"></param>
            void SetPathToTarget((bool condition, List<Transform> targets) result, int targetIndex, int subDivide)
            {
                _targets = (result.condition) ? result.targets : new();
                if (_targets.Count > 0)
                    _path = DetectPath(transform.position, _targets[targetIndex].position, subDivide).ToList();
            }

            /// <summary> すでに検出した道筋をたどる </summary>
            void TraceCurrentPath()
            {
                if (_path == null) return; // if path is null, return and do nothing
                _aiLimb.MoveToPoint(_path[_pathIndex], _moveSpeed);
                var result = CheckInsideOfBorder(transform.position, _path[_pathIndex], 1, 1);
                if (result) _pathIndex = (_pathIndex + 1 < _path.Count) ? _pathIndex + 1 : _pathIndex;
            }

            private void Start()
            {
                _aiLimb = GetComponent<AILimbComponent>();
                SetPathToTarget(IsTargetInLevel(), _targetIndex, 10);
            }
            private void FixedUpdate()
            {
                TraceCurrentPath();
            }
            private void OnDrawGizmos()
            {
                // visible path
                Gizmos.color = Color.black;
                Gizmos.DrawLineStrip(_path.ToArray(), true);
                foreach (var path in _path)
                {
                    Gizmos.DrawWireSphere(path, _pathAvoidanceRad);
                }
            }
        }
    }
}