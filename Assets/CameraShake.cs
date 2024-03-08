using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    public CinemachineVirtualCamera cinemachine;
    public CinemachineBasicMultiChannelPerlin shake;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        shake = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public async void impulseShake(float intensity, float duration)
    {
        shake.m_AmplitudeGain = intensity;
        await Boss.Delay(duration);
        shake.m_AmplitudeGain = 0;
    }

    public void Shake()
    {
        shake.m_AmplitudeGain = 1;
    }

    public void Stop()
    {
        shake.m_AmplitudeGain = 0;
    }
}
