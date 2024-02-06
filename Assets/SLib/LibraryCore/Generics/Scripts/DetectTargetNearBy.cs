// �Ǘ��� ����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace SgLib
{
    enum DetectingMode
    {
        LayerMask,
        Transform,
    }
    /// <summary> ����̑Ώۂ��C�ӂ̍��W�̋߂��ɂ���̂����m�������̃C�x���g </summary>
    public class DetectTargetNearBy : MonoBehaviour
    {
        // ���m�����̂Ƃ�`�i���j�̔��a
        [SerializeField, Range(0, 5),
            Header("���E���̔��a�B\n�X���C�_�[�𓮂����ƁA���E�����L����")]
        float borderRadius;
        // ���m�Ώۂ̃��C���[�}�X�N
        [SerializeField,
            Header("���m�Ώۂ̃��C���[�Ƀ`�F�b�N�{�b�N�X������΂悢")]
        LayerMask targetLayer;
        // ���m�Ώۂ̃g�����X�t�H�[��
        [SerializeField,
            Header("���m���[�h��Transform�̏ꍇ\n���m�Ώۂ̂������Ƀh���b�O�A���h�h���b�v�Ŋ��蓖�Ă�")]
        Transform targetTransform;
        // ���m�������̃C�x���g
        [SerializeField,
            Header("�����Ɍ��m�Ώۂ����m�����ꍇ\n���΂��Ăق����C�x���g�����蓖�Ă�")]
        UnityEvent eventDetectedInsideBorder;
        // ���m�����̒��S�̃g�����X�t�H�[��
        [SerializeField, Header("���m���[�h��Transform�̎�\n���m����W�i�I�u�W�F�N�g�j�������Ɋ��蓖�Ă�")]
        Transform centerTransform;
        // ���m���[�h
        [SerializeField, Header("=���m���[�h=\n" +
            "LayerMask:\n�A�^�b�`�����I�u�W�F�N�g�̍��W�����ƂɌ��m����\n�P�̃I�u�W�F�N�g�̐N���̂݌��m����Ƃ��Ɏg���Ƃ悢\n" +
            "Transform:\n���蓖�Ă����C���[�̕s���萔�̃I�u�W�F�N�g�̐N�������m����Ƃ��Ɏg���Ƃ悢")]
        DetectingMode mode;
        private void Update()
        {
            switch (mode)
            {
                case DetectingMode.LayerMask:
                    if (Physics.CheckSphere(centerTransform.position, borderRadius, targetLayer))
                        eventDetectedInsideBorder?.Invoke();
                    break;
                case DetectingMode.Transform:
                    if ((targetTransform.position - centerTransform.position).sqrMagnitude < borderRadius * borderRadius)
                        eventDetectedInsideBorder?.Invoke();
                    break;
                default:
                    throw new System.Exception("���m���[�h�����蓖�Ă��Ă��܂���");
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(centerTransform.position, borderRadius);
        }
#endif
    }
}