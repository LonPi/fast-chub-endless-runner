using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    float yMax = 0.5f;
    float yMin = -0.5f;
    float xCenter;

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            if (Player.Instance.PlayerDeath()) xCenter = 0f;
            else xCenter = 0.8f;
            transform.position = Vector3.Lerp(transform.position, target.position, 0.1f) + new Vector3(xCenter, 0.3f, -20f);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, yMin, yMax), transform.position.z);
        }
    }
}
