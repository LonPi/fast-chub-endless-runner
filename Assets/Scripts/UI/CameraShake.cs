using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    Camera cameraObj;

    float shakeAmount = 0;

    void Awake()
    {
        cameraObj = GetComponent<Camera>();
    }

    public void Shake(float amt, float length)
    {
        shakeAmount = amt;
        InvokeRepeating("DoShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void DoShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 camPos = cameraObj.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            //float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x = offsetX;
            camPos.y = 0f;
            camPos.z = 0f;
            cameraObj.transform.localPosition = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        cameraObj.transform.localPosition = Vector3.zero;
    }

}
