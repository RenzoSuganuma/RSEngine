using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineAnimSample : MonoBehaviour
{
    // �X�v���C���A�j���[�V����������R���|�[�l���g�������ɃA�^�b�`
    [SerializeField] SplineAnimate _splineAnim;    
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 100), "Start_Spline_Anim"))
        {
            _splineAnim.Play();
        }
    }
}