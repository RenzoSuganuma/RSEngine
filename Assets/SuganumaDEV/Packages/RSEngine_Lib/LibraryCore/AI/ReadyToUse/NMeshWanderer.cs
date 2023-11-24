// ä«óùé“ êõè¿
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
namespace RSEngine
{
    namespace AI
    {
        /// <summary> NavMeshAgent ÇégÇ¡ÇΩÅAèÑâÒåoòHÇïKóvÇ∆ÇµÇ»Ç¢ÅAÅhúpújé“ÅhAIã@î\ÇíÒãüÇ∑ÇÈ </summary>
        [RequireComponent(typeof(NavMeshAgent))]
        public class NMeshWanderer : MonoBehaviour
        {
            [SerializeField, Range(1, 50)] float _sightRange;
            [SerializeField, Range(1, 50)] float _AttackingRange;
            [SerializeField, Range(1, 50)] float _patrollingRange;
            [SerializeField] UnityEvent _patrollingEvent;
            [SerializeField] UnityEvent _chasingEvent;
            [SerializeField] UnityEvent _attakingEvent;
            [SerializeField] LayerMask _targetLayer;
            [SerializeField] LayerMask _groundLayer;
            [SerializeField] int _groundLayerNum;
            NavMeshAgent _agent = default;
            Transform _currentTarget;
            Vector3 _currentDestination = Vector3.zero;
            bool _isTargetFound = false;
            bool _isInAttackingRange = false;
            bool _movePointSet = false;
            #region Core Behaviours
            /// <summary> Return Transforms Witch Inside Range </summary>
            /// <param name="range"></param>
            /// <param name="layerMask"></param>
            /// <returns></returns>
            Transform[] GetTargetsInsidesRange(float range, LayerMask layerMask)
            {
                return Array.ConvertAll(Physics.OverlapSphere(transform.position, range, layerMask), x => x.transform);
            }
            /// <summary> Return Condition Is The Objects Inside Range </summary>
            /// <param name="range"></param>
            /// <param name="layerMask"></param>
            /// <returns></returns>
            bool IsTargetInsideRange(float range, LayerMask layerMask)
            {
                return Physics.CheckSphere(transform.position, range, layerMask);
            }
            /// <summary> Find Patroll Point And Return It </summary>
            /// <param name="maxDIstance"></param>
            /// <param name="range"></param>
            /// <param name="groundLayer"></param>
            /// <returns></returns>
            Vector3 CheckPatrollPoint(float maxDIstance, float range, LayerMask groundLayer)
            {
                var rx = UnityEngine.Random.Range(-range, range);
                var rz = UnityEngine.Random.Range(-range, range);
                var point = new Vector3(transform.position.x + rx, transform.position.y, transform.position.z + rz);
                if (Physics.Raycast(point, -transform.up, maxDIstance, _groundLayer))
                    return point;
                return transform.position;
            }
            #endregion
            #region  Behaviours
            /// <summary> Patrolling Behaviour </summary>
            void Patroll()
            {
                if (!_movePointSet)
                {
                    _currentDestination = CheckPatrollPoint(2, _sightRange, _groundLayer);
                    _agent.SetDestination(_currentDestination);
                    _movePointSet = true;
                }
                if (Vector3.Distance(transform.position, _currentDestination) < 1)
                {
                    _agent.SetDestination(transform.position);
                    _movePointSet = false;
                }
            }
            /// <summary> Chasing Behaviour </summary>
            void Chase()
            {
                if (_movePointSet) { _movePointSet = false; }
                _currentTarget = GetTargetsInsidesRange(_sightRange, _targetLayer)[0];
                var dis = Vector3.Distance(transform.position, _currentTarget.position);
                if (dis > 1)
                {
                    _agent.SetDestination(_currentTarget.position);
                }
                else
                {
                    _agent.SetDestination(transform.position);
                }
            }
            /// <summary> Stop Moving </summary>
            void Stop()
            {
                _agent.SetDestination(transform.position);
            }
            #endregion
            private void Start()
            {
                _agent = GetComponent<NavMeshAgent>();
            }
            private void FixedUpdate()
            {
                _isTargetFound = IsTargetInsideRange(_sightRange, _targetLayer);
                _isInAttackingRange = IsTargetInsideRange(_AttackingRange, _targetLayer);
                if (!_isTargetFound && !_isInAttackingRange || _movePointSet) // Patroll
                {
                    Patroll();
                    _patrollingEvent.Invoke();
                }
                if (_isTargetFound && !_isInAttackingRange) // Chase
                {
                    Chase();
                    _chasingEvent.Invoke();
                }
                if (_isTargetFound && _isInAttackingRange) // Attack
                {
                    Stop();
                    _attakingEvent.Invoke();
                }
            }
            private void OnDrawGizmos()
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _sightRange);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _AttackingRange);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, _patrollingRange);
                Gizmos.DrawWireCube(_currentDestination, Vector3.one * .5f);
            }
        }
    }
}