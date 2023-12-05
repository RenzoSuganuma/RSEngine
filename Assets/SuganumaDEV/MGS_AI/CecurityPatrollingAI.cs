using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CecurityPatrollingAI : MonoBehaviour
{
    [SerializeField, Range(0,180f)] float _fSightRange;
    [SerializeField] Transform _tLookAtTarget;
    [SerializeField] Transform _tTarget;
    [SerializeField] float _fSightDistanceLimit;

    private void Update()
    {
        if (CaplureNow())
        {
            Debug.Log("Captured!");
        }
    }

    bool CaplureNow()
    {
        Vector3 vLook = _tLookAtTarget.position - transform.position;
        Vector3 vTarget = _tTarget.position - transform.position;
        float fCosHalfSight = Mathf.Cos(_fSightRange / 2 * Mathf.Deg2Rad);
        float fCosTarget = Vector3.Dot(vLook, vTarget) / (vLook.magnitude * vTarget.magnitude);
        return fCosTarget > fCosHalfSight && vTarget.magnitude < _fSightDistanceLimit;
    }

    void OnDrawGizmos()
    {
        // 視界の範囲（正面及び左右の端）をギズモとして描く
        Vector3 lookAtDirection = (_tLookAtTarget.position - this.transform.position).normalized;    // 正面（正規化）
        Vector3 rightBorder = Quaternion.Euler(0, _fSightRange / 2, 0) * lookAtDirection;    // 右端（正規化）
        Vector3 leftBorder = Quaternion.Euler(0, -1 * _fSightRange / 2, 0) * lookAtDirection;    // 左端（正規化）
        Gizmos.color = Color.cyan;  // 正面は水色で描く
        Gizmos.DrawRay(this.transform.position, lookAtDirection * _fSightDistanceLimit);
        Gizmos.color = Color.blue;  // 両端は青で描く
        Gizmos.DrawRay(this.transform.position, rightBorder * _fSightDistanceLimit);
        Gizmos.DrawRay(this.transform.position, leftBorder * _fSightDistanceLimit);
        // 扇形もしくはコーン状の図形を gizmo あるいはゲーム上に表示したい場合は、
        // Mesh を動的に生成して Gizmos.DrawMesh を使う
    }
}
