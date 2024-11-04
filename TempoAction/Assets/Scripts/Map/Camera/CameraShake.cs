using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    private static CameraShake instance;
    [SerializeField] CameraShakeProfile Profile;
    [SerializeField] CinemachineImpulseListener Listener;

    private CinemachineImpulseDefinition impulseDefinition;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        instance = this;

        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (impulseSource == null)
            impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
    }

    public void TestShake()
    {
        Shake(Profile);
    }
    public void Shake(CameraShakeProfile Profile)
    {
        impulseDefinition = impulseSource.m_ImpulseDefinition;
        impulseDefinition.m_ImpulseDuration = Profile.ShakeTime;
        impulseSource.m_DefaultVelocity = Profile.Velocity;
        impulseDefinition.m_ImpulseShape = Profile.ShakeCustom ? CinemachineImpulseDefinition.ImpulseShapes.Custom : CinemachineImpulseDefinition.ImpulseShapes.Bump;
        impulseDefinition.m_CustomImpulseShape = Profile.ShakeCurve;


        Listener.m_ReactionSettings.m_AmplitudeGain = Profile.ShakeAmplitude;
        Listener.m_ReactionSettings.m_FrequencyGain = Profile.ShakeFrequency;
        Listener.m_ReactionSettings.m_Duration = Profile.ShakeDuration;

        impulseSource.GenerateImpulseWithForce(Profile.ShakeForce);
    }
}

