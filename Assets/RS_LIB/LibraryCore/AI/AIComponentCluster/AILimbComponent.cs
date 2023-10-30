using UnityEngine;
namespace RSEngine
{
    namespace AI
    {
        /// <summary> �l�̂ł����l���̋@�\��񋟂���N���X�B�ړ����i�� </summary>
        public class AILimbComponent : MonoBehaviour
        {
            /// <summary> �n���ꂽ���W��ڎw���Đi�� </summary>
            /// <param name="point"></param>
            /// <param name="speed"></param>
            public void MoveToPoint(Vector3 point, float speed)
            {
                var dir = (point - transform.position).normalized;
                dir.y = 0;
                var vel = dir * speed * Time.deltaTime;
                transform.position += vel;
            }
        }
    }
}