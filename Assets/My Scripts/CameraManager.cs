using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraManager : MonoBehaviour, IBomb
{
    private Vector3 _cameraInitialPosition;
    private float _shakeMagnitude = 0.2f;
    private float _shakeTime = 0.2f;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void ShakeIt()
    {
        _cameraInitialPosition = transform.position;
        InvokeRepeating(nameof(StartCameraShaking), 0.1f, 0.001f);
        Invoke(nameof(StopCameraShaking), _shakeTime);
    }

    private void StartCameraShaking()
    {
        float cameraShakingOffsetX = Random.value * _shakeMagnitude * 2 - _shakeMagnitude;
        float cameraShakingOffsetZ = Random.value * _shakeMagnitude * 2 - _shakeMagnitude;
        Vector3 cameraIntermadiatePosition = transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
        cameraIntermadiatePosition.z += cameraShakingOffsetZ;
        transform.position = cameraIntermadiatePosition;
    }

    private void StopCameraShaking()
    {
        CancelInvoke(nameof(StartCameraShaking));
        transform.position = _cameraInitialPosition;
    }

    public void Bomb()
    {
        ShakeIt();
    }
}
