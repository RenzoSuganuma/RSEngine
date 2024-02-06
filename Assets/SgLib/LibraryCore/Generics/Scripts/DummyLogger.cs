// �Ǘ��� ����
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SgLib
{
    enum LoggingMode
    {
        Normal,
        Warning,
        Error,
    }
    /// <summary> �n���ꂽ�����������Debug���O�֏o�͂���B </summary>
    public class DummyLogger : MonoBehaviour
    {
        [SerializeField] LoggingMode mode;
        public void DummyLoggerOutputLog(string message)
        {
            switch (mode)
            {
                case LoggingMode.Warning:
                    Debug.LogWarning(message);
                    break;
                case LoggingMode.Error:
                    Debug.LogError(message);
                    break;
                default:
                    Debug.Log(message);
                    break;
            }
        }
    }
}