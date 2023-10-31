using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        /// <summary> �l�̂ł����l���̋@�\��񋟂���N���X�B�ړ����i�� </summary>
        public class AILimbComponent : MonoBehaviour
        {
            [SerializeField, Header("Y Axis Offset")] float _yOffset;
            /// <summary> �n���ꂽ���W��ڎw���Đi�� </summary>
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