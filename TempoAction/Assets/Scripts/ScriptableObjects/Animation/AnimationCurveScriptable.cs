using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationCurve", menuName = "ScriptableObjects/Animation/Curve/Curve", order = 1)]
public class AnimationCurveScriptable : ScriptableObject
{
    public CurveClass[] curves;
}

[Serializable]
public class CurveClass
{
    [Header("커브 이름")]
    public string curveName;
    [Header("X축 커브")]
    public AnimationCurve xCurve;
    [Header("X축 이동거리")]
    public float xDis;
    [Header("Y축 커브")]
    public AnimationCurve yCurve;
    [Header("Y축 이동거리")]
    public float yDis;
    [Header("커브가 끝날 때 까지 걸리는 시간")]
    public float curveTime;
}
