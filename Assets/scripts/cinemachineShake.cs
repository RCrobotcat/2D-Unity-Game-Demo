using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cinemachineShake : MonoBehaviour
{
    public static cinemachineShake instance { get; private set; } // 单例模式

    CinemachineVirtualCamera cinemachineVirtualCamera;
    float shakeTimer;
    float shakeTimerTotal;
    float startingIntensity;

    private void Awake()
    {
        instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
                cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                /*cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f; // 确保震动强度被重置为0*/
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
            /*else
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
                cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
                // 震动强度, Lerp 插值函数用来平滑地改变值
            }*/
        }
    }

    public void shakingCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity; // 震动强度

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }
}
