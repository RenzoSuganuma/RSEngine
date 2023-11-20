using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RSEngine
{
    enum LoggingMode
    {
        Normal,
        Warning,
        Error,
    }
    /// <summary> ìnÇ≥ÇÍÇΩï∂éöóÒÇÇΩÇæDebugÉçÉOÇ÷èoóÕÇ∑ÇÈÅB </summary>
    public class DummyLogger : MonoBehaviour
    {
        [SerializeField] LoggingMode _mode;
        public void DummyLoggerOutputLog(string message)
        {
            switch (_mode)
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