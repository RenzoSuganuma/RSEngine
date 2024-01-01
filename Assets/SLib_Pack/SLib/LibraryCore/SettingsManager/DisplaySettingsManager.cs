using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// �S�� ����
// �f�B�X�v���C�̃f�o�C�X���ƃ��t���b�V�����[�g�̕ύX�͂ł������ۂ� �� �����܂ł͓���m�F�ł��Ă���
// Player�ݒ� �� FullScreen ���[�h �A Default Is Native Resolution = false ���̐ݒ�͕K�����邱��
namespace SLib
{
    public class DisplaySettingsManager : MonoBehaviour
    {
        public string GetResolution()
        {
            return $"{Screen.currentResolution.ToString()}";
        }

        public string GetDisplayName(int index)
        {
            List<DisplayInfo> list = new List<DisplayInfo>();
            Screen.GetDisplayLayout(list);
            return list[index].name;
        }

        /// <summary> �󔒋�؂�Ńs�N�Z�����̎w������� </summary>
        /// <param name="resolution"></param>
        public void SetDisplayResolutions(string resolution)
        {
            int width;
            int height;
            width = int.Parse(resolution.Split()[0]);
            height = int.Parse(resolution.Split()[1]);

            Screen.SetResolution(width, height, true);
        }

        public int GetRefreshRate()
        {
            return Application.targetFrameRate;
        }

        public void SetRefreshRate(int rate)
        {
            Application.targetFrameRate = rate;
        }
    }
}
