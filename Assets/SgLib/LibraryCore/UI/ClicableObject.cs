using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

#region �Q�l����
// https://qiita.com/sune2/items/cf9ef9d197b47b2d7a10
// https://www.nttpc.co.jp/technology/number_algorithm.html
#endregion

namespace SgLib.UI
{
    /// <summary> 
    /// <para> �{�^���̂悤�ȋ@�\��񋟂��� </para>
    /// This Component Works As Button
    /// </summary>
    public class ClicableObject : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasRaycastFilter
    {
        [SerializeField]
        UnityEvent OnClick;

        Image _image;

        List<Vector2> _verts = new();

        void Press()
        {
            if (OnClick != null)
                OnClick.Invoke();
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            var img = _image;
            if (img == null) { _image = GetComponent<Image>(); }
            Vector2 local;
            var rectT = (RectTransform)transform;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectT, sp, eventCamera, out local))
            {
                // ��ʓ���RectTransform�̕��ʂ�ray���q�b�g�����true �����ł̓q�b�g���Ȃ������ꍇ
                return false;
            }
            var rect = rectT.rect;
            // �X�v���C�g���ɓ��������ꍇ�A���̍��W�����߂�
            var pivot = rectT.pivot;
            var sprite = _image.sprite;
            var x = (local.x / rect.width + pivot.x - .5f) * sprite.rect.width / sprite.pixelsPerUnit;
            var y = (local.y / rect.height + pivot.y - .5f) * sprite.rect.height / sprite.pixelsPerUnit;
            var p = new Vector2(x, y);
            // ���O����
            var physicShapeCnt = sprite.GetPhysicsShapeCount();
            for (int i = 0; i < physicShapeCnt; i++)
            {
                sprite.GetPhysicsShape(i, _verts);
                if (IsInside(_verts, p))
                {
                    return true;
                }
            }
            return false;
        }
        float Cross2D(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        // ���_�Ɠ_����n���ē��O���������
        bool IsInside(List<Vector2> verts, Vector2 point)
        {
            #region ���߂������A���S���Y��
#if false
            int cnt = 0;
            for (int i = 0; i < verts.Count - 1; i++)
            {
                // ������̕� �[ �_�������������ɂ��āA�n�_�ƏI�_�̊Ԃɂ���I�_���܂�ł��Ȃ�
                if (((verts[i].y <= point.y) && (verts[i + 1].y > point.y))
                    // �������̕� �[ �_�������������ɂ��āA�n�_�ƏI�_�̊Ԃɂ���A�n�_���܂�ł��Ȃ�
                    || (verts[i].y > point.y) && (verts[i + 1].y <= point.y))
                {
                    var vt = (point.y - verts[i].y) / (verts[i + 1].y - verts[i].y);
                    // �ӂ͓_�������E���ɂ��邪�d�Ȃ�Ȃ�
                    // �ӂ��_���Ɠ��������ɂȂ�ʒu�����A���̎��̂��̒l�Ɠ_���̂��̒l���r
                    // �����������ꍇ�i�����ɂ���j
                    if (point.x < (verts[i].x + (vt * verts[i + 1].x - verts[i].x)))
                    {
                        ++cnt;
                    }
                }
            }
            return !(cnt % 2 == 0); // �����񐔂������̏ꍇ�ɂ͓����ɂ͂Ȃ�
#endif
            #endregion
            var n = verts.Count;
            var isInside = false;
            for (int i = 0; i < n; i++)
            {
                var nxt = i + 1;
                if (nxt >= n) nxt = 0;

                var A = verts[i] - point;
                var B = verts[nxt] - point;

                if (A.y > B.y)
                    (A.y, B.y) = (B.y, A.y);    // swap

                if (A.y <= 0 && 0 < B.y && Cross2D(A, B) > 0)
                    isInside = !isInside;
            }
            return isInside;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (PointerEventData.InputButton.Left == eventData.button)
            {
                Debug.Log("Click");
                Press();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Debug.Log("Submit");
        }

        private void Awake()
        {
        }

        private void Update()
        {
        }
    }
}
