using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class CameraEffect : MonoBehaviour
{
    public AnalogGlitch analogGlitch;
    public DigitalGlitch digitalGlitch;

    private void Awake()
    {
        digitalGlitch = GetComponent<DigitalGlitch>();
        analogGlitch = GetComponent<AnalogGlitch>();
    }

    public void DeadEffect()
    {
        analogGlitch.scanLineJitter = 0.4f;
        analogGlitch.horizontalShake = 0.03f;
    }

    public void ReplayEffect()
    {
        digitalGlitch.intensity = 0.05f;
    }
}
