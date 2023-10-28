using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
namespace RSEngine
{
    namespace AI
    {
        /// <summary> l‘Ì‚Å‚¢‚¤”]‚É“–‚½‚é‹@”\‚ğ’ñ‹Ÿ‚·‚é </summary>
        [RequireComponent(typeof(AILimbComponent))]
        public class AIBrainComponent : MonoBehaviour
        {
            [SerializeField] Transform _target;
            [SerializeField] Transform _referencePoint;
            AILimbComponent _aiLimb;
            Vector3[] _path = new Vector3[0];
            Vector3[] SetPath(Vector3 start, Vector3 end, int pointCount)
            {
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
            private void Start()
            {
                SetPath(transform.position, _target.position, 5).ToList().ForEach(x => new GameObject().transform.position = x);
            }
            private void FixedUpdate()
            {
            }
        }
    }
}