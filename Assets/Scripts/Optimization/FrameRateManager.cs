using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    [Header("Frame Settings")] private int MaxRate = 9999;
    public float TargetFrameRate = 60.0f;
    private float CurrentFrameTime;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        CurrentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine(WaitForNextFrame());
    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            CurrentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleeptime = CurrentFrameTime - t - 0.01f;
            if (sleeptime > 0)
                Thread.Sleep((int)(sleeptime * 1000));
            while (t < CurrentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
}