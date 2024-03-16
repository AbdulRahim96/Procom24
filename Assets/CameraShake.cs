using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.DemiLib;
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

    /*void FakeDelay(float pow = 1)
    {
        GameObject dot = GameObject.Find(".");
        dot.transform.DOMoveX(1, 0.15f).OnComplete(() =>
        {
            playerTrigger.Attacking(power * pow);
            if (pow == 1)
                CameraShake.instance.impulseShake(0.3f, 0.3f);
            else
                CameraShake.instance.impulseShake(1, 0.3f);
        });
    }*/
}
