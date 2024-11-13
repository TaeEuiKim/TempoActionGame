using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShake", menuName = "ScriptableObjects/Camera Shake", order = 0)]
public class CameraShakeProfile : ScriptableObject
{
    [Header("Shake Settings")]
    [Tooltip("Èçµé¸² ½Ã°£")]
    public float ShakeTime = 0.2f;
    [Tooltip("Èçµé¸² Èû")]
    public float ShakeForce = 1.0f;
    [Tooltip("Èçµé¸² ¹üÀ§")]
    public Vector3 Velocity = new Vector3(0, -1f, 0);
    [Tooltip("Èçµé¸² Ä¿½ºÅÒ")]
    public bool ShakeCustom = false;
    [Tooltip("Èçµé¸² ¼Óµµ °î¼±")]
    public AnimationCurve ShakeCurve;

    [Space]
    [Tooltip("Èçµé¸² ÁøÆø")]
    public float ShakeAmplitude = 1.0f;
    [Tooltip("Èçµé¸² ºóµµ")]
    public float ShakeFrequency = 1.0f;
    [Tooltip("Èçµé¸² Áö¼Ó")]
    public float ShakeDuration = 1.0f;
}
