#define DEBUGGING
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        /// <summary> �l�̂ł����]�ɓ�����@�\��񋟂��� </summary>
        [RequireComponent(typeof(AILimbComponent))]
        public class AIBrainComponent : MonoBehaviour
        {
            [SerializeField, Header("Targets")] List<Transform> _targets = new();
            int _targetIndex = 0;
            [SerializeField, Header("Target Layer Number")] int _targetLayerNum;
            [SerializeField, Header("MoveSpeed"), Range(1, 10)] float _moveSpeed;
            [SerializeField, Header("Path")] List<Vector3> _path;
            [SerializeField, Header("Obstacle For AI Layer")] LayerMask _obstacleLayer;
            [SerializeField, Header("Path Point Obstacle Avoidance")] float _pathAvoidanceRad;
            int _pathIndex = 0;
            AILimbComponent _aiLimb;
            /// <summary> ���ǂ铹�؂��n�_�ƏI�_���w�肵�Ċ���o�� </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <param name="pointCount"> �n�_�I�_�܂߂����ԍ��W�_ </param>
            /// <returns></returns>
            Vector3[] DetectPath(Vector3 start, Vector3 end, int pointCount) // 0 ~ pointcount - 1
            {
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
                for (int i = 0; i < pointCount - 1; i++)
                {
                    if (Physics.CheckSphere(path[i], _pathAvoidanceRad, _obstacleLayer))
                    {
                        var cols = Physics.OverlapSphere(path[i], _pathAvoidanceRad, _obstacleLayer);
                        var vec = -(cols[0].transform.position - path[i]);
                        vec.y = 0; // temporary formatting 
                        path[i] += vec * 1.5f;
                    } // if is there obstacles 
                } // check each point's near in obstacles
                return path;
            }
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
            /// <summary> ���x���Ƀ^�[�Q�b�g�����邩�̌����������������Ń^�[�Q�b�g�����^�[������ </summary>
            /// <returns></returns>
            (bool condition, List<Transform> targets) IsTargetInLevel()
            {
                var list = GameObject.FindObjectsOfType<Transform>().Where(x => x.gameObject.layer == _targetLayerNum).ToList();
                return (list.Count > 0, list);
            }
            /// <summary> �^�[�Q�b�g�܂ł̓��؂����o���� </summary>
            /// <param name="result"></param>
            /// <param name="targetIndex"></param>
            /// <param name="subDivide"></param>
            void SetPathToTarget((bool condition, List<Transform> targets) result, int targetIndex, int subDivide)
            {
                _targets = (result.condition) ? result.targets : new();
                if (_targets.Count > 0)
                    _path = DetectPath(transform.position, _targets[targetIndex].position, subDivide).ToList();
            }
            /// <summary> ���łɌ��o�������؂����ǂ� </summary>
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