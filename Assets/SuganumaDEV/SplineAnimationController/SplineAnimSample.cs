using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineAnimSample : MonoBehaviour
{
    // スプラインアニメーションをするコンポーネントをここにアタッチ
    [SerializeField] SplineAnimate _splineAnim;    
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 100), "Start_Spline_Anim"))
        {
            _splineAnim.Play();
        }
    }
}