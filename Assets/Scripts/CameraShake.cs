using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public float _shakeIntensity = 0.5f;
    public float _shakeDuration = 0.5f;
    public static CameraShake Instance;
    
    private void Awake() {
        Instance = this;
    }
    public void ShakeCamera()
    {
        iTween.ShakePosition(gameObject, iTween.Hash(
            "x", _shakeIntensity,
            "y", _shakeIntensity,
            "time", _shakeDuration));
    }
}
