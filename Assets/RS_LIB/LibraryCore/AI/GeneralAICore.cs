using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RSEngine
{
    [RequireComponent(typeof(Rigidbody))]
    public class GeneralAICore : MonoBehaviour
    {
        Rigidbody _rb;
        [SerializeField, Range(1f, 50f)] float _targetDetectRangeRadius;
        [SerializeField, Range(0, 5)] float _targetDetectBufferRangeRadius;
        [SerializeField, Range(1f, 50f)] float _attackingRangeRadius;
        [SerializeField, Range(1f, 100f)] float _moveSpeed;
        [SerializeField, Range(1f, 10f)] float _fallSpeed;
        [SerializeField] Transform[] _patrollPath;
        [SerializeField] List<GameObject> _targetsList = new();
        [SerializeField] LayerMask _targetObjectLayer;
        [SerializeField] LayerMask _groundLayer;
        [SerializeField] int _targetsLayerNumber;
        int _currentTargetIndex = 0;
        int _currentPatrollPathIndex = 0;
        bool _isGrounded = false;
        void AddGravityToThis() => _rb.AddForce((!_isGrounded) ? -transform.up * _fallSpeed * 100 : Vector3.zero);
        /// <summary> 接地検出 </summary>
        bool CheckGrounded()
        {
            return Physics.Raycast(transform.position, -transform.up, 1f, _groundLayer);
        }
        bool IsTargetInLevel()
        {
            return GameObject.FindObjectsOfType<GameObject>().Where(x => x.layer == _targetsLayerNumber).ToList().Count > 0;
        }
        /// <summary> 追跡可能なすべての敵の検索 </summary>
        List<GameObject> DetectChasableTargets()
        {
            return GameObject.FindObjectsOfType<GameObject>().Where(x => x.layer == _targetsLayerNumber).ToList();
        }
        /// <summary> 任意のオブジェクトが検知圏内にいるか判定 </summary>
        /// <param name="start"> 始点 </param>
        /// <param name="end"> 終点 </param>
        /// <param name="limitDistance"> 検知半径 </param>
        /// <param name="limitOffset"> 検知距離誤差許容値 </param>
        /// <returns></returns>
        bool CheckInsideBorder(Vector3 start, Vector3 end, float limitDistance, float limitOffset)// 距離ベース検知
        {
            float dx = end.x - start.x;
            float dy = end.y - start.y;
            float dz = end.z - start.z;
            float dd = dx * dx + dy * dy + dz * dz;
            float lim = limitDistance * limitDistance;
            return dd < lim + limitOffset;
        }
        #region 行動ルーチン
        /// <summary> 渡された道筋をたどるルーチン </summary>
        /// <param name="patrollPointIndex"></param>
        /// <param name="patrollPath"></param>
        void PatrollNow(int patrollPointIndex, Transform[] patrollPath)
        {
            var dir = (patrollPath[patrollPointIndex].position - transform.position).normalized;
            dir.y = 0;
            var v = dir * _moveSpeed;
            _rb.velocity = v;
            var b = CheckInsideBorder(transform.position, patrollPath[patrollPointIndex].position, 1, 1);
            Debug.Log($"{gameObject.name}:{this.GetType()}:inside border-{b},index-{patrollPointIndex},point-{patrollPath[patrollPointIndex]}");
            if (b) { _currentPatrollPathIndex = (_currentPatrollPathIndex + 1 < _patrollPath.Length) ? _currentPatrollPathIndex + 1 : 0; }
        }
        /// <summary> 渡されたオブジェクトを追跡するルーチン </summary>
        /// <param name="isTargetInSight"></param>
        /// <param name="target"></param>
        void ChaseWithTarget(bool isTargetInSight, GameObject target)
        {
            if (isTargetInSight)
            {
                var dir = (target.transform.position - transform.position).normalized;
                dir.y = 0;
                var v = dir * _moveSpeed;
                _rb.velocity = v;
            }
        }
        #endregion
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            _targetsList = DetectChasableTargets();
        }
        void Update()
        {

        }
        void FixedUpdate()
        {
            _isGrounded = CheckGrounded();
            AddGravityToThis();
            if (IsTargetInLevel())
            {
                var b = CheckInsideBorder(transform.position, _targetsList[_currentTargetIndex].transform.position, _targetDetectRangeRadius, _targetDetectBufferRangeRadius);
                ChaseWithTarget(b, _targetsList[_currentTargetIndex]);
            }
            else
            {
                Debug.Log("Target Is Not Found");
                PatrollNow(_currentPatrollPathIndex, _patrollPath);
            }
        }
        void OnDrawGizmos()
        {
            // Sightable Target Detect Range
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _targetDetectRangeRadius);
            // Sightable Attack Range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackingRangeRadius);
            // Sightable Patroll Path
            Gizmos.color = Color.blue;
            Gizmos.DrawLineStrip(Array.ConvertAll(_patrollPath, x => x.position), true);
        }
    }
}